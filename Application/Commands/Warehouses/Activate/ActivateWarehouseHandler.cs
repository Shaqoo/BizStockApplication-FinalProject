using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Warehouses.Activate
{
    public class ActivateWarehouseHandler : IRequestHandler<ActivateWarehouseCommand, Result<string>>
    {
        private readonly IWarehouseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogRepository _logRepository;
        private readonly IAuthService _authService;
        private readonly ILogger<ActivateWarehouseHandler> _logger;
        public ActivateWarehouseHandler(IWarehouseRepository repository,IUnitOfWork unitOfWork,IAuditLogRepository logRepository
            ,IAuthService authService,ILogger<ActivateWarehouseHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(ActivateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _repository.GetByIdAsync(request.WarehouseId);

            if (warehouse is null)
                return Result<string>.Failure("Warehouse not found.");

            if (warehouse.IsActive)
                return Result<string>.Success("Warehouse is already active.");

            await _unitOfWork.BeginTransactionAsync();
            warehouse.Activate(); 
            await _repository.UpdateWarehouseAsync(warehouse);
            await _unitOfWork.CommitTransactionAsync();

            await _logRepository.AddAsync(new AuditLog(
                _authService.CurrentUser()!.Id,
                "ActivateWarehouse",
                nameof(Warehouse),
                warehouse.Id,
                $"Warehouse '{warehouse.Name}' was activated.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));
            _logger.LogInformation("Warehouse with ID {WarehouseId} activated successfully.", warehouse.Id);
            return Result<string>.Success("Warehouse activated successfully.");
        }
    }

}
