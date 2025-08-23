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

namespace Application.Commands.StockMovements.TransferStock
{
    public class TransferStockHandler(IAuthService authService,
        IStockMovementRepository stockMovementRepository,
        IProductRepository productRepository,
        IWarehouseItemRepository warehouseItemRepository,
        IAuditLogRepository auditLogRepository,
        IMediator mediator,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork,
        ILogger<TransferStockHandler> logger) : IRequestHandler<TransferStockCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(TransferStockCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("User not authenticated.");

            var product = await productRepository.GetByIdAsync(request.Request.ProductId);
            if (product == null)
                return Result<string>.Failure("Product not found.");

            var fromWarehouseItem = await warehouseItemRepository.GetByExpression(a => a.WarehouseId == request.Request.FromWarehouseId
            && a.ProductId == request.Request.ProductId);
            if (fromWarehouseItem == null)
                return Result<string>.Failure("From warehouse item not found.");

            if (fromWarehouseItem.Quantity < request.Request.Quantity)
                return Result<string>.Failure("Insufficient stock in from warehouse.");

            var toWarehouseItem = await warehouseItemRepository.GetByExpression(a => a.WarehouseId == request.Request.ToWarehouseId
            && a.ProductId == request.Request.ProductId);

            await unitOfWork.BeginTransactionAsync();
            if (toWarehouseItem is null)
            {
                toWarehouseItem = new WarehouseItem(request.Request.ToWarehouseId, request.Request.ProductId, fromWarehouseItem.ReorderLevel, 0);
                await warehouseItemRepository.AddAsync(toWarehouseItem);
            }

            fromWarehouseItem.DecreaseStock(request.Request.Quantity);
            toWarehouseItem.IncreaseStock(request.Request.Quantity);

            await warehouseItemRepository.UpdateWarehouseItemAsync(fromWarehouseItem);
            await warehouseItemRepository.UpdateWarehouseItemAsync(toWarehouseItem);
            var stockMovement = new StockMovement(
                fromWarehouseItem.Id,
                StockMovementType.TransferOut,
                request.Request.Quantity,
                request.Request.Reason,
                user.Id
            );

            var stockMovementTo = new StockMovement(
                toWarehouseItem.Id,
                StockMovementType.TransferIn,
                request.Request.Quantity,
                request.Request.Reason,
                user.Id
            );


            await stockMovementRepository.AddAsync(stockMovement);
            await stockMovementRepository.AddAsync(stockMovementTo);
            await unitOfWork.CommitTransactionAsync();

            await auditLogRepository.AddAsync(new AuditLog(user.Id, "TransferStock", nameof(StockMovement), stockMovement.Id,
                $"Transferred {request.Request.Quantity} units of Product {product.Name} (ID: {product.Id}) from Warehouse {fromWarehouseItem.Warehouse.Name} (ID: {request.Request.FromWarehouseId}) to Warehouse {toWarehouseItem.Warehouse.Name} (ID: {request.Request.ToWarehouseId}). Reason: {request.Request.Reason}",
                request.RequestMetadata.IpAddress, request.RequestMetadata.UserAgent));

            logger.LogInformation("Stock transfer completed successfully from {FromWarehouseId} to {ToWarehouseId} for product {ProductId}.",
                request.Request.FromWarehouseId, request.Request.ToWarehouseId, request.Request.ProductId);

            await publishEndpoint.Publish(new StockTransferredEvent(product.Id, request.Request.FromWarehouseId, request.Request.ToWarehouseId, request.Request.Quantity
                ,product.Name,fromWarehouseItem.Warehouse.Name,toWarehouseItem.Warehouse.Name, request.Request.Reason),cancellationToken);

            await mediator.Publish(new StockTransferredEvent(product.Id, request.Request.FromWarehouseId, request.Request.ToWarehouseId, request.Request.Quantity,
                product.Name, fromWarehouseItem.Warehouse.Name, toWarehouseItem.Warehouse.Name, request.Request.Reason), cancellationToken);
            return Result<string>.Success("Stock transfer completed successfully.");
        }
    }
}
