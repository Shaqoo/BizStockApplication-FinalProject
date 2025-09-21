using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.AddPurchaseOrderItem
{
    public class AddPurchaseOrderItemCommandHandler(
    IPurchaseOrderRepository purchaseOrderRepository,
    IPurchaseOrderItemRepository purchaseOrderItemRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditLogRepository auditLogRepository,
    ILogger<AddPurchaseOrderItemCommandHandler> logger,
    IMediator mediator
) : IRequestHandler<AddPurchaseOrderItemCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AddPurchaseOrderItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.Dto;

                var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(dto.PurchaseOrderId);
                if (purchaseOrder == null)
                    return Result<Guid>.Failure("Purchase order not found.");

                if (purchaseOrder.Status != PurchaseOrderStatus.Draft)
                    return Result<Guid>.Failure("Items can only be added to purchase orders in Draft status.");

                if (!await productRepository.Exists(dto.ProductId))
                    return Result<Guid>.Failure("Product not found.");

                var item = new PurchaseOrderItem(dto.ProductId, dto.ProductName, dto.QuantityOrdered, dto.UnitPrice, purchaseOrder.Id);
                purchaseOrder.RecalculateSubTotal();
                await purchaseOrderItemRepository.AddAsync(item);
                await unitOfWork.SaveChangesAsync();

                var user = authService.CurrentUser();
                await auditLogRepository.AddAsync(new AuditLog(
                    user!.Id,
                    "AddPurchaseOrderItem",
                    nameof(PurchaseOrderItem),
                    item.Id,
                    $"Added item {dto.ProductName} (x{dto.QuantityOrdered}) to PO {purchaseOrder.OrderNumber}",
                    request.Metadata.IpAddress,
                    request.Metadata.UserAgent
                ));

                await mediator.Publish(new PurchaseOrderItemAddedEvent(purchaseOrder.Id,purchaseOrder.OrderNumber,purchaseOrder.SupplierId, item.Id, dto.ProductName,dto.QuantityOrdered,dto.UnitPrice));

                logger.LogInformation("Item {Product} added to PO {PO}", dto.ProductName, purchaseOrder.OrderNumber);

                return Result<Guid>.Success(item.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding item to purchase order.");
                return Result<Guid>.Failure("An error occurred while adding the item.");
            }
        }
    }

}
