using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Warehouses.Create
{
    public class CreateWarehouseHandler(IWarehouseRepository warehouseRepository,
        IUnitOfWork unitOfWork,
        IAuditLogRepository auditLogRepository,
        ILogger<CreateWarehouseHandler> logger,
        IAuthService authService,
        IMediator mediator,
        IPublishEndpoint publishEndpoint
        ) : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
    {
        public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.WarehouseDto == null)
            {
                return Result<WarehouseDto>.Failure("Invalid request payload.");
            }
            var isUnique = await warehouseRepository.IsNameUnique(request.WarehouseDto.Name);
            if (isUnique)
            {
                return Result<WarehouseDto>.Failure("Warehouse name is already in use.");
            }

            var warehouse = new Warehouse(
                request.WarehouseDto.Name,
                request.WarehouseDto.Location
            );
            await unitOfWork.BeginTransactionAsync();
            try
            {
                await warehouseRepository.AddAsync(warehouse);
                await unitOfWork.CommitTransactionAsync();
                var warehouseDto = new WarehouseDto(warehouse.Id, warehouse.Name, warehouse.Location,true,0);
                await auditLogRepository.AddAsync(new AuditLog(
                    authService.CurrentUser()!.Id,
                    "Warehouse Creation",
                    nameof(Warehouse),
                    warehouse.Id,
                    $"Warehouse '{warehouse.Name}' was created.",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                    ));
                await publishEndpoint.Publish(new WarehouseCreatedEvent(warehouse.Id,warehouse.Name, warehouse.Location));
                await mediator.Publish(new WarehouseCreatedEvent(warehouse.Id, warehouse.Name, warehouse.Location), cancellationToken);
                return Result<WarehouseDto>.Success(warehouseDto);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                await auditLogRepository.AddAsync(new AuditLog(
                    authService.CurrentUser().Id,
                    "Warehouse Creation Error",
                    nameof(Warehouse),
                    null,
                    $"Error creating warehouse: {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));
                logger.LogError(ex, "Error creating warehouse");
                return Result<WarehouseDto>.Failure("An error occurred while creating the warehouse.");
            }
        }
    }
}
