using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Products.AddQuantity
{
    public class AddProductQuantityCommandHandler : IRequestHandler<AddProductQuantityCommand, Result<string>>
    {
        private readonly IWarehouseItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<AddProductQuantityCommandHandler> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly IStockMovementRepository _stockMovementRepository;

        public AddProductQuantityCommandHandler(
            IStockMovementRepository stockMovementRepository,
            IWarehouseItemRepository repository,
            IUnitOfWork unitOfWork,
            IAuditLogRepository auditLogRepository,
            ILogger<AddProductQuantityCommandHandler> logger,
            IWarehouseRepository warehouseRepository,
            IProductRepository productRepository,
            IMediator mediator,
            IAuthService authService)
        {
            _stockMovementRepository = stockMovementRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _authService = authService;
            _mediator = mediator;
        }

        public async Task<Result<string>> Handle(AddProductQuantityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.Dto;

                var item = await _repository.GetByExpression(a => a.WarehouseId == dto.WarehouseId &&
                a.ProductId == dto.ProductId);

                var product = await _productRepository.GetByIdAsync(dto.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} does not exist.", dto.ProductId);
                    return Result<string>.Failure("Product does not exist.");
                }

                await _unitOfWork.BeginTransactionAsync();
                if (item is null)
                {
                    var warehouse = await _warehouseRepository.Exists(dto.WarehouseId);
                    if(!warehouse)
                    {
                        _logger.LogWarning("Warehouse with ID {WarehouseId} does not exist.", dto.WarehouseId);
                        await _unitOfWork.RollbackTransactionAsync();
                        return Result<string>.Failure("Warehouse does not exist.");
                    }

                    item = new WarehouseItem(dto.WarehouseId, dto.ProductId, dto.ReorderLevel, dto.Quantity);
                    await _repository.AddAsync(item);
                    _logger.LogInformation("Created new WarehouseItem for Product {ProductId} in Warehouse {WarehouseId}", dto.ProductId, dto.WarehouseId);
                }
                else
                {
                    item.IncreaseStock(dto.Quantity);
                    item.SetReorderLevel(dto.ReorderLevel);
                    await _repository.UpdateWarehouseItemAsync(item);
                    _logger.LogInformation("Updated existing WarehouseItem for Product {ProductId} in Warehouse {WarehouseId}", dto.ProductId, dto.WarehouseId);
                }

                await _stockMovementRepository.AddAsync(new StockMovement(item.Id,StockMovementType.Inbound,
                    dto.Quantity,"New Product Was Added",_authService.CurrentUser()!.Id));

                await _unitOfWork.CommitTransactionAsync();

                await _auditLogRepository.AddAsync(new AuditLog(
                    userId: _authService.CurrentUser()!.Id,
                    action: "AddProductQuantity",
                    entityName: "WarehouseItem",
                    entityId: item.Id,
                    details: $"Added {dto.Quantity} units and set reorder level to {dto.ReorderLevel} for product {dto.ProductId} in warehouse {dto.WarehouseId}.",
                    ip: request.Metadata.IpAddress,
                    userAgent: request.Metadata.UserAgent
                ));
                await _mediator.Publish(new ProductQuantityAddedEvent(dto.ProductId,product.Name,dto.Quantity,
                   _authService.CurrentUser()!.Id));

                return Result<string>.Success("Product quantity added/updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product quantity for Product {ProductId} in Warehouse {WarehouseId}", request.Dto.ProductId, request.Dto.WarehouseId);
                return Result<string>.Failure("Failed to add product quantity.");
            }
        }
    }


}
