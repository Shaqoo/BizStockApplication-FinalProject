using Application.Commands.Warehouses.Activate;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Warehouses.Deactivate
{
    public class DeactivateWarehouseHandler : IRequestHandler<DeactivateWarehouseCommand, Result<string>>
    {
        private readonly IWarehouseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogRepository _logRepository;
        private readonly IAuthService _authService;
        private readonly ILogger<DeactivateWarehouseHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        public DeactivateWarehouseHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork, IAuditLogRepository logRepository
            , IAuthService authService, ILogger<DeactivateWarehouseHandler> logger, IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _authService = authService;
            _logger = logger;
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<string>> Handle(DeactivateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _repository.GetByIdAsync(request.WarehouseId);  

            if (warehouse is null)
                return Result<string>.Failure("Warehouse not found.");

            if (!warehouse.IsActive)
                return Result<string>.Success("Warehouse is already inactive.");

            var hasItems = await _repository.HasItemAsync(request.WarehouseId);
            if (hasItems)
                return Result<string>.Failure("Warehouse cannot be deactivated because it still contains products.");

            await _unitOfWork.BeginTransactionAsync();

            warehouse.Deactivate();

            await _repository.UpdateWarehouseAsync(warehouse);

            await _unitOfWork.CommitTransactionAsync();

            await _logRepository.AddAsync(new AuditLog(
                _authService.CurrentUser()!.Id,
                "DeactivateWarehouse",
                nameof(Warehouse),
                warehouse.Id,
                $"Warehouse '{warehouse.Name}' was deactivated.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));
            _logger.LogInformation("Warehouse with ID {WarehouseId} deactivated successfully.", warehouse.Id);

            await _publishEndpoint.Publish(new WarehouseDeactivatedEvent(warehouse.Id, warehouse.Name,warehouse.Location));
            await _mediator.Publish(new WarehouseDeactivatedEvent(warehouse.Id, warehouse.Name, warehouse.Location), cancellationToken);

            _logger.LogInformation("Warehouse deactivation event published for warehouse ID {WarehouseId}.", warehouse.Id);

            return Result<string>.Success("Warehouse deactivated successfully.");
        }
    }

}
