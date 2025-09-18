using Application.Commands.Products.AddQuantity;
using Application.Commands.PurchaseOrders.ReceivePurchaseOrderItems;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.ReceivePurchaseOrder
{
    public class ReceivePurchaseOrderItemsHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IAuditLogRepository auditLogRepository,
        IWarehouseRepository warehouseRepository,
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ReceivePurchaseOrderItemsHandler> logger
    ) : IRequestHandler<ReceivePurchaseOrderItemsCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ReceivePurchaseOrderItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId);
                if (warehouse == null)
                {
                    logger.LogWarning("Warehouse {WarehouseId} not found", request.WarehouseId);
                    return Result<bool>.Failure("Warehouse not found");
                }

                logger.LogInformation("Starting to receive items for PurchaseOrder {PurchaseOrderId}", request.PurchaseOrderId);

                var po = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
                if (po == null)
                {
                    logger.LogWarning("PurchaseOrder {PurchaseOrderId} not found", request.PurchaseOrderId);
                    return Result<bool>.Failure("Purchase Order not found");
                }

                if (po.Status != PurchaseOrderStatus.Confirmed &&
                    po.Status != PurchaseOrderStatus.PartiallyReceived)
                {
                    logger.LogWarning("Cannot receive items for PurchaseOrder {PurchaseOrderId} with status {Status}", po.Id, po.Status);
                    return Result<bool>.Failure($"Cannot receive items. PO status is {po.Status}");
                }

                foreach (var dto in request.Items)
                {
                    var poItem = po.Items.FirstOrDefault(i => i.Id == dto.PurchaseOrderItemId);
                    if (poItem == null)
                    {
                        logger.LogWarning("PO Item {PurchaseOrderItemId} not found in PO {PurchaseOrderId}", dto.PurchaseOrderItemId, po.Id);
                        return Result<bool>.Failure($"Item {dto.PurchaseOrderItemId} not found in Purchase Order");
                    }

                    if (poItem.QuantityReceived + dto.QuantityReceived > poItem.QuantityOrdered)
                    {
                        logger.LogWarning("Received quantity {ReceivedQty} exceeds ordered quantity {OrderedQty} for PO {PurchaseOrderId}, Item {PurchaseOrderItemId}",
                            dto.QuantityReceived, poItem.QuantityOrdered, po.Id, poItem.Id);
                        return Result<bool>.Failure($"Quantity received exceeds ordered quantity for item {poItem.ProductName}");
                    }

                    poItem.Receive(dto.QuantityReceived);

                    logger.LogInformation("Received {QtyReceived} units for Item {PurchaseOrderItemId} (Product {ProductId}) in PO {PurchaseOrderId}",
                        dto.QuantityReceived, poItem.Id, poItem.ProductId, po.Id);
                }

                
                var newStatus = po.Items.All(i => i.IsFullyReceived)
                    ? PurchaseOrderStatus.Received
                    : PurchaseOrderStatus.PartiallyReceived;

                po.UpdateStatus(newStatus);

                await unitOfWork.SaveChangesAsync();

                logger.LogInformation("Updated PurchaseOrder {PurchaseOrderId} status to {Status}", po.Id, newStatus);

               
                var user = authService.CurrentUser();
                var audit = new AuditLog(
                    user!.Id,
                    "ReceivePurchaseOrderItems",
                    nameof(PurchaseOrder),
                    po.Id,
                    $"Received {request.Items.Sum(x => x.QuantityReceived)} items into Warehouse {request.WarehouseId} for PO {po.OrderNumber}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );
                await auditLogRepository.AddAsync(audit);

                logger.LogInformation("Audit log created for receiving items into Warehouse {WarehouseId} for PO {PurchaseOrderId}", request.WarehouseId, po.Id);

                var header = httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString();
                var ip = httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();

                foreach (var dto in request.Items)
                {
                    var poItem = po.Items.First(i => i.Id == dto.PurchaseOrderItemId);

                    await mediator.Send(new AddProductQuantityCommand(new AddProductQuantityDto
                    {
                        ProductId = poItem.ProductId,
                        Quantity = dto.QuantityReceived, 
                        ReorderLevel = 0,
                        WarehouseId = request.WarehouseId,
                    }, new RequestMetadata(header ?? string.Empty, ip)));

                    logger.LogInformation("Stock updated for Product {ProductId}, Quantity {Qty}, Warehouse {WarehouseId}", poItem.ProductId, dto.QuantityReceived, request.WarehouseId);
                }

               
                await mediator.Publish(new PurchaseOrderItemsReceivedEvent(
                    po.Id,
                    po.OrderNumber,
                    po.SupplierId,
                    request.Items.Select(i => new ReceivedItemEventDto(i.PurchaseOrderItemId, i.QuantityReceived)).ToList()
                ), cancellationToken);

                logger.LogInformation("Published PurchaseOrderItemsReceivedEvent for PurchaseOrder {PurchaseOrderId}", po.Id);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error receiving items for PurchaseOrder {PurchaseOrderId}", request.PurchaseOrderId);
                return Result<bool>.Failure("An error occurred while receiving items.");
            }
        }
    }
}
