using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.UpdatePurchaseOrderItem
{
    public class UpdatePurchaseOrderItemCommandHandler(
    IPurchaseOrderItemRepository purchaseOrderItemRepository,
    IPurchaseOrderRepository purchaseOrderRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditLogRepository auditLogRepository,
    ILogger<UpdatePurchaseOrderItemCommandHandler> logger,
    IMediator mediator
) : IRequestHandler<UpdatePurchaseOrderItemCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdatePurchaseOrderItemCommand request, CancellationToken cancellationToken)
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

                item.Update(dto.QuantityOrdered, dto.UnitPrice);

                await unitOfWork.SaveChangesAsync();

                var user = authService.CurrentUser();
                await auditLogRepository.AddAsync(new AuditLog(
                    user!.Id,
                    "UpdatePurchaseOrderItem",
                    nameof(PurchaseOrderItem),
                    item.Id,
                    $"Updated item {item.ProductName} in PO {item.PurchaseOrderId} (Qty: {dto.QuantityOrdered}, Price: {dto.UnitPrice})",
                    request.Metadata.IpAddress,
                    request.Metadata.UserAgent
                ));

                await mediator.Publish(new PurchaseOrderItemUpdatedEvent(item.PurchaseOrderId,purchaseOrder.OrderNumber,purchaseOrder.SupplierId, item.Id, dto.QuantityOrdered,dto.UnitPrice));

                logger.LogInformation("Item {Item} updated in PO {PO}", item.ProductName, item.PurchaseOrderId);

                return Result<Guid>.Success(item.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating purchase order item.");
                return Result<Guid>.Failure("An error occurred while updating the item.");
            }
        }
    }

}
