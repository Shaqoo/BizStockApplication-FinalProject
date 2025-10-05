using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StockMovements.ReserveStock
{
    public class ReserveStockCommandHandler(IStockMovementRepository stockMovementRepository,
        IProductRepository productRepository,
        IWarehouseItemRepository warehouseItemRepository,
        IMediator mediator,
        ILogger<ReserveStockCommandHandler> logger,
        IUnitOfWork unitOfWork) : IRequestHandler<ReserveStockCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var item in request.Items)
                {
                    var product = await productRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                        return Result<Unit>.Failure($"Product {item.ProductId} not found.");

                    var warehouseItems = product.StockByWarehouse;
                    if (warehouseItems == null || !warehouseItems.Any())
                        return Result<Unit>.Failure($"No warehouse stock for product {item.ProductId}.");

                    int remainingQty = item.Quantity;
                    foreach (var warehouseItem in warehouseItems)
                    {
                        if (remainingQty <= 0) break;
                        var reserveQty = Math.Min(warehouseItem.Quantity, remainingQty);
                        if (reserveQty <= 0) continue;

                        warehouseItem.DecreaseStock(reserveQty);
                        await warehouseItemRepository.UpdateWarehouseItemAsync(warehouseItem);

                        var movement = new StockMovement(
                            warehouseItem.Id,
                            StockMovementType.Outbound,
                            reserveQty,
                            $"Reserved for Sales Order {request.SalesOrderId}",
                            null
                        );
                        await stockMovementRepository.AddAsync(movement);

                        remainingQty -= reserveQty;
                    }

                    if (remainingQty > 0)
                        throw new InvalidOperationException($"Insufficient stock to reserve {item.Quantity} of product {item.ProductId}");
                }

                await unitOfWork.CommitTransactionAsync();

                await mediator.Publish(new StockReservedEvent(request.SalesOrderId,request.Items), cancellationToken);
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Failed to reserve stock for order {SalesOrderId}", request.SalesOrderId);
                return Result<Unit>.Failure("Failed to reserve stock: " + ex.Message);
            }
        }

    }
}
