using Application.Commands.PurchaseOrders.ReceivePurchaseOrderItems;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.ReceivePurchaseOrder
{
    public class ReceivePurchaseOrderItemsHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IAuditLogRepository auditLogRepository,
        IMediator mediator,
        ILogger<ReceivePurchaseOrderItemsHandler> logger
    ) : IRequestHandler<ReceivePurchaseOrderItemsCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ReceivePurchaseOrderItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var po = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
                if (po == null)
                {
                    logger.LogWarning("Purchase Order {PurchaseOrderId} not found", request.PurchaseOrderId);
                    return Result<bool>.Failure("Purchase Order not found");
                }

                if (po.Status != PurchaseOrderStatus.Confirmed &&
                    po.Status != PurchaseOrderStatus.PartiallyReceived)
                {
                    logger.LogWarning("Cannot receive items for Purchase Order {PurchaseOrderId} with status {Status}",
                        po.Id, po.Status);
                    return Result<bool>.Failure($"Cannot receive items. PO status is {po.Status}");
                }

                foreach (var dto in request.Items)
                {
                    var poItem = po.Items.FirstOrDefault(i => i.Id == dto.PurchaseOrderItemId);
                    if (poItem == null)
                    {
                        logger.LogWarning("PO Item {PurchaseOrderItemId} not found in PO {PurchaseOrderId}",
                            dto.PurchaseOrderItemId, po.Id);
                        return Result<bool>.Failure($"Item {dto.PurchaseOrderItemId} not found in Purchase Order");
                    }

                    if (poItem.QuantityReceived + dto.QuantityReceived > poItem.QuantityOrdered)
                    {
                        logger.LogWarning("Received qty exceeds ordered qty for PO {PurchaseOrderId}, Item {PurchaseOrderItemId}",
                            po.Id, poItem.Id);
                        return Result<bool>.Failure($"Quantity received exceeds ordered quantity for item {poItem.ProductName}");
                    }

                    poItem.Receive(dto.QuantityReceived); 
                }

                if (po.Items.All(i => i.IsFullyReceived))
                    po.UpdateStatus(PurchaseOrderStatus.Received);
                else
                    po.UpdateStatus(PurchaseOrderStatus.PartiallyReceived);

                await unitOfWork.SaveChangesAsync();

            
                var user = authService.CurrentUser();
                var audit = new AuditLog(
                    user!.Id,
                    "ReceivePurchaseOrderItems",
                    nameof(PurchaseOrder),
                    po.Id,
                    $"Received items for PO {po.OrderNumber}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );
                await auditLogRepository.AddAsync(audit);

                await mediator.Publish(new PurchaseOrderItemsReceivedEvent(
                    po.Id,
                    po.OrderNumber,
                    po.SupplierId,
                    request.Items.Select(i => new ReceivedItemEventDto(i.PurchaseOrderItemId, i.QuantityReceived)).ToList()
                ), cancellationToken);

                logger.LogInformation("Items received for PO {PurchaseOrderId}", po.Id);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error receiving items for PO {PurchaseOrderId}", request.PurchaseOrderId);
                return Result<bool>.Failure("An error occurred while receiving items.");
            }
        }
    }
}
