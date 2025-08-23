using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Warehouses.Update
{
    public class UpdateWarehouseCommandHandler(
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateWarehouseCommandHandler> logger)
    : IRequestHandler<UpdateWarehouseCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await warehouseRepository.GetByIdAsync(request.Id);
            if (warehouse is null)
            {
                logger.LogWarning("Update failed: Warehouse with ID {WarehouseId} not found.", request.Id);
                return Result<Guid>.Failure("Warehouse not found.");
            }

            var checck = await warehouseRepository.IsNameUnique(request.Update.Name);
            if (!checck && warehouse.Location != request.Update.Location)
            {
                logger.LogWarning("Update failed: Warehouse name {WarehouseName} is already in use.", request.Update.Name);
                return Result<Guid>.Failure("Warehouse name is already in use.");
            }

            warehouse.Update(request.Update.Name, request.Update.Location);

            await unitOfWork.BeginTransactionAsync();
            await warehouseRepository.UpdateWarehouseAsync(warehouse);
            await unitOfWork.CommitTransactionAsync();

            logger.LogInformation("Warehouse with ID {WarehouseId} updated successfully.", warehouse.Id);

            return Result<Guid>.Success(warehouse.Id);
        }
    }

}
