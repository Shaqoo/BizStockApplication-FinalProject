using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.CreatePurchaseOrder
{
    public class CreatePurchaseOrderHandler(
    IProductRepository productRepository,
    IPurchaseOrderRepository purchaseOrderRepository,
    IPurchaseOrderItemRepository purchaseOrderItemRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditLogRepository auditLogRepository,
    IMediator mediator,
    ILogger<CreatePurchaseOrderHandler> logger,
    ISupplierRepository supplierRepository
) : IRequestHandler<CreatePurchaseOrderCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.CreatePurchaseOrderDto;

                var supplier = await supplierRepository.GetByIdAsync(dto.SupplierId);
                if (supplier == null)
                {
                    logger.LogWarning("Supplier Not Found");
                    return Result<Guid>.Failure("Supplier Not Found");
                }

                await unitOfWork.BeginTransactionAsync();

                var nextNumber = await purchaseOrderRepository.GenerateNextOrderNumber();
                var purchaseOrder = new PurchaseOrder(nextNumber, supplier.Id, dto.Discount, dto.Tax, dto.ExpectedDeliveryDate, dto.Notes);
                await purchaseOrderRepository.AddAsync(purchaseOrder);

                foreach (var item in dto.Items)
                {
                    if (!await productRepository.Exists(item.ProductId))
                    {
                        await unitOfWork.RollbackTransactionAsync();
                        logger.LogWarning("Product {ProductId} not found", item.ProductId);
                        return Result<Guid>.Failure($"Product {item.ProductId} not found.");
                    }

                    var orderItem = new PurchaseOrderItem(item.ProductId, item.ProductName, item.QuantityOrdered, item.UnitPrice, purchaseOrder.Id);
                    await purchaseOrderItemRepository.AddAsync(orderItem);
                }


                await unitOfWork.CommitTransactionAsync();


                var user = authService.CurrentUser();
                var audit = new AuditLog(
                    user!.Id,
                    "CreatePurchaseOrder",
                    nameof(PurchaseOrder),
                    purchaseOrder.Id,
                    $"Created purchase order {purchaseOrder.OrderNumber} for supplier {purchaseOrder.SupplierId} with {purchaseOrder.Items.Count} items.",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );

                await auditLogRepository.AddAsync(audit);

                var orderEvent = new CreatePurchaseOrderEvent(purchaseOrder.Id, purchaseOrder.OrderNumber, purchaseOrder.SupplierId, purchaseOrder.DateCreated.Date, purchaseOrder.ExpectedDeliveryDate, purchaseOrder.Discount, purchaseOrder.Tax);
                foreach (var item in dto.Items)
                {
                    orderEvent.AddItem(new CreatePurchaseOrderItemEvent(item.ProductId, item.ProductName, item.QuantityOrdered, item.UnitPrice));
                }
                await mediator.Publish(orderEvent);

                return Result<Guid>.Success(purchaseOrder.Id);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error creating purchase order");
                return Result<Guid>.Failure("An error occurred while creating the purchase order.");
            }
        }
    }

}
