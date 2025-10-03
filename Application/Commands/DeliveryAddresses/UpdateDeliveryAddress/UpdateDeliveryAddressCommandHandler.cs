using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.DeliveryAddresses.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressCommandHandler
        : IRequestHandler<UpdateDeliveryAddressCommand, Result<bool>>
    {
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateDeliveryAddressCommandHandler> _logger;
        private readonly IAuthService _authService;

        public UpdateDeliveryAddressCommandHandler(
            IDeliveryAddressRepository deliveryAddressRepository,
            IAuthService authService,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateDeliveryAddressCommandHandler> logger)
        {
            _deliveryAddressRepository = deliveryAddressRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _authService = authService;
        }

        public async Task<Result<bool>> Handle(UpdateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var address = await _deliveryAddressRepository.GetByIdAsync(request.Request.Id);
                if (address is null)
                {
                    _logger.LogWarning("Delivery address {AddressId} not found", request.Request.Id);
                    return Result<bool>.Failure("Delivery address not found");
                }

                if (request.Request.IsDefault)
                    address.SetDefault(true);
                if(request.Request.StateId > 0 && request.Request.StateId <= 37)
                    address.ChangeState(request.Request.StateId);
                if (request.Request.LgaId > 0 && request.Request.LgaId <= 774)
                    address.ChangeLga(request.Request.LgaId);
                if(!string.IsNullOrWhiteSpace(request.Request.Landmark))
                    address.UpdateLandmark(request.Request.Landmark);
                if(!string.IsNullOrWhiteSpace(request.Request.Street))
                    address.UpdateStreet(request.Request.Street);
                if(!string.IsNullOrWhiteSpace(request.Request.PostalCode))
                    address.UpdatePostalCode(request.Request.PostalCode);
                if (!string.IsNullOrWhiteSpace(request.Request.AdditionalPhoneNumber))
                    address.ChangeDetails(address.Email,address.FullName,address.AdditionalPhoneNumber,address.PhoneNumber);
                 
                await _deliveryAddressRepository.UpdateAsync(address);

                var auditLog = new AuditLog(
                    _authService.CurrentUser()!.Id,
                    "UpdateDeliveryAddress",
                    nameof(DeliveryAddress),
                    address.Id,
                    $"Updated delivery address {address.Street}, LGA {address.LgaId}, State {address.StateId}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );

                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Delivery address {AddressId} updated successfully", address.Id);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating delivery address {AddressId}", request.Request.Id);
                return Result<bool>.Failure("An error occurred while updating delivery address");
            }
        }
    }

}
