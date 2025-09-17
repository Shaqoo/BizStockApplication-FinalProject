using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.RemovePurchaseOrderItem
{
    public class RemovePurchaseOrderItemCommandHandler(
    IPurchaseOrderRepository purchaseOrderRepository,
    IPurchaseOrderItemRepository purchaseOrderItemRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditLogRepository auditLogRepository,
    ILogger<RemovePurchaseOrderItemCommandHandler> logger,
    IMediator mediator
) : IRequestHandler<RemovePurchaseOrderItemCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RemovePurchaseOrderItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.Dto;

                var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(dto.PurchaseOrderId);
                if (purchaseOrder == null)
                    return Result<Guid>.Failure("Purchase order not found.");

                if (purchaseOrder.Status != PurchaseOrderStatus.Draft)
                    return Result<Guid>.Failure("Items can only be added to purchase orders in Draft status.");


                var item = await purchaseOrderItemRepository.GetByIdAsync(dto.PurchaseOrderItemId);
                if (item == null)
                    return Result<Guid>.Failure("Item not found.");

                await purchaseOrderItemRepository.DeleteItemAsync(item);
                await unitOfWork.SaveChangesAsync();

                var user = authService.CurrentUser();
                await auditLogRepository.AddAsync(new AuditLog(
                    user!.Id,
                    "RemovePurchaseOrderItem",
                    nameof(PurchaseOrderItem),
                    item.Id,
                    $"Removed item {item.ProductName} from PO {item.PurchaseOrderId}",
                    request.Metadata.IpAddress,
                    request.Metadata.UserAgent
                ));

                await mediator.Publish(new PurchaseOrderItemRemovedEvent(item.PurchaseOrderId, item.Id,purchaseOrder.OrderNumber,purchaseOrder.SupplierId));

                logger.LogInformation("Item {Item} removed from PO {PO}", item.ProductName, item.PurchaseOrderId);

                return Result<Guid>.Success(item.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing purchase order item.");
                return Result<Guid>.Failure("An error occurred while removing the item.");
            }
        }
    }

}
