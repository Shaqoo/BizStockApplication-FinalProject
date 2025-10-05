using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using global::Application.Dto;
using global::Application.Interfaces.Repository;
using global::Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.StockMovements.RestoreStock
{
    
    public class RestoreStockCommandHandler : IRequestHandler<RestoreStockCommand, Result<Unit>>
    {
        private readonly IWarehouseItemRepository _warehouseItemRepository;
        private readonly IStockMovementRepository _stockMovementRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RestoreStockCommandHandler> _logger;

        public RestoreStockCommandHandler(
            IWarehouseItemRepository warehouseItemRepository,
            IMediator mediator,
            IStockMovementRepository stockMovementRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ILogger<RestoreStockCommandHandler> logger)
        {
            _warehouseItemRepository = warehouseItemRepository;
            _stockMovementRepository = stockMovementRepository;
            _mediator = mediator;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(RestoreStockCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", item.ProductId);
                    return Result<Unit>.Failure($"Product with ID {item.ProductId} not found.");
                }

                var warehouseItem = product.StockByWarehouse.FirstOrDefault();
                if (warehouseItem == null)
                {
                    _logger.LogWarning("No warehouse items found for product ID {ProductId}.", item.ProductId);
                    return Result<Unit>.Failure($"No warehouse items found for product ID {item.ProductId}.");
                }

                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    warehouseItem.IncreaseStock(item.Quantity);
                    await _warehouseItemRepository.UpdateWarehouseItemAsync(warehouseItem);

                    var stockMovement = new StockMovement(
                        warehouseItem.Id,
                        StockMovementType.Inbound,
                        item.Quantity,
                        $"Restored for canceled Sales Order {request.SalesOrderId}",
                        null
                    );
                    await _stockMovementRepository.AddAsync(stockMovement);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogError(ex, "Error restoring stock for product ID {ProductId}.", item.ProductId);
                    return Result<Unit>.Failure($"Error restoring stock for product ID {item.ProductId}: {ex.Message}");
                }
            }

            await _unitOfWork.CommitTransactionAsync();

            await _mediator.Publish(new StockRestoredEvent(request.SalesOrderId, request.Items), cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}

