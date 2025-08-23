using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StockMovements.AdjustInventory
{
    public class AdjustStockHandler(IWarehouseItemRepository warehouseItemRepository,
        IUnitOfWork unitOfWork,
        IAuditLogRepository auditLogRepository,
        ILogger<AdjustStockHandler> logger,
        IProductRepository productRepository,
        IPublishEndpoint publishEndpoint,
        IMediator mediator,
        IAuthService authService) : IRequestHandler<AdjustStockCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("User not authenticated.");

            var product = await productRepository.GetByIdAsync(request.Request.ProductId);
            if (product == null)
                return Result<string>.Failure("Product not found.");

            var warehouseItem = await warehouseItemRepository.GetByExpression(a => a.WarehouseId == request.Request.WarehouseId
            && a.ProductId == request.Request.ProductId);

            if (warehouseItem == null)
                return Result<string>.Failure("Warehouse item not found.");

            await unitOfWork.BeginTransactionAsync();

            try
            {
                if (request.Request.AdjustmentType == AdjustmentType.Increase)
                {
                    warehouseItem.IncreaseStock(request.Request.Quantity);
                }
                else
                {
                    if (warehouseItem.Quantity < request.Request.Quantity)
                        return Result<string>.Failure("Insufficient stock for adjustment.");

                    warehouseItem.DecreaseStock(request.Request.Quantity);
                }

                await warehouseItemRepository.UpdateWarehouseItemAsync(warehouseItem);
                var stockMovement = new StockMovement(
                    warehouseItem.Id,
                    request.Request.AdjustmentType == AdjustmentType.Increase ? StockMovementType.AdjustmentIn : StockMovementType.AdjustmentOut,
                    request.Request.Quantity,
                    request.Request.Reason,
                    user.Id
                );

                await unitOfWork.CommitTransactionAsync();

                await auditLogRepository.AddAsync(new AuditLog(user.Id, "AdjustStock", nameof(StockMovement), stockMovement.Id,
                    $"Adjusted stock for Product {product.Name} (ID: {product.Id}) in Warehouse {warehouseItem.Warehouse.Name} (ID: {request.Request.WarehouseId}). Adjustment Type: {request.Request.AdjustmentType}, Quantity: {request.Request.Quantity}, Reason: {request.Request.Reason}",
                    request.RequestMetadata.IpAddress, request.RequestMetadata.UserAgent));

                logger.LogInformation("Stock adjusted successfully for ProductId: {ProductId}, WarehouseId: {WarehouseId}, AdjustmentType: {AdjustmentType}, Quantity: {Quantity}",
                    request.Request.ProductId, request.Request.WarehouseId, request.Request.AdjustmentType, request.Request.Quantity);

                var @event = new StockAdjustedManuallyEvent
                {
                    ProductId = request.Request.ProductId,
                    WarehouseId = request.Request.WarehouseId,
                    WarehouseName = warehouseItem.Warehouse.Name,
                    FinalQuantity = warehouseItem.Quantity,
                    QuantityChanged = request.Request.Quantity,
                    ProductName = product.Name,
                    PerformedBy = user.Email,
                    Reason = request.Request.Reason
                };

                await mediator.Publish(@event, cancellationToken);
                await publishEndpoint.Publish(@event, cancellationToken);

                return Result<string>.Success("Stock adjusted successfully.");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();

                await auditLogRepository.AddAsync(new AuditLog(user.Id, "AdjustStock", nameof(StockMovement), null,
                    $"Error adjusting stock for Product {product.Name} (ID: {request.Request.ProductId}) in Warehouse {warehouseItem.Warehouse.Name} (ID: {request.Request.WarehouseId}). Adjustment Type: {request.Request.AdjustmentType}, Quantity: {request.Request.Quantity}, Reason: {request.Request.Reason}. Error: {ex.Message}",
                    request.RequestMetadata.IpAddress, request.RequestMetadata.UserAgent));

                logger.LogError(ex, "Error adjusting stock for ProductId: {ProductId}, WarehouseId: {WarehouseId}, AdjustmentType: {AdjustmentType}, Quantity: {Quantity}",
                    request.Request.ProductId, request.Request.WarehouseId, request.Request.AdjustmentType, request.Request.Quantity);

                return Result<string>.Failure("Error adjusting stock. Please try again later.");

            }
        }
    }
}
