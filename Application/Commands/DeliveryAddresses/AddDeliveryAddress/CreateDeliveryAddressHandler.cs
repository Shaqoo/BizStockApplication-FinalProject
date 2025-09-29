using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.DeliveryAddresses.AddDeliveryAddress
{
    public class CreateDeliveryAddressCommandHandler
        : IRequestHandler<CreateDeliveryAddressCommand, Result<Guid>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateDeliveryAddressCommandHandler> _logger;
        private readonly IAuthService _authService;

        public CreateDeliveryAddressCommandHandler(
            ICustomerRepository customerRepository,
            IAuthService authService,
            IDeliveryAddressRepository deliveryAddressRepository,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateDeliveryAddressCommandHandler> logger)
        {
            _authService = authService;
            _customerRepository = customerRepository;
            _deliveryAddressRepository = deliveryAddressRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(request.Request.CustomerId);
                if (customer is null)
                {
                    _logger.LogWarning("Customer not found when adding delivery address");
                    return Result<Guid>.Failure("Customer not found");
                }
                if (request.Request.IsDefault)
                    customer.ClearDefaultDeliveryAddresses();

                var address = DeliveryAddress.Create(
                    request.Request.CustomerId,
                    request.Request.StateId,
                    request.Request.LgaId,
                    request.Request.Street,
                    request.Request.IsDefault,
                    request.Request.Landmark,
                    request.Request.PostalCode
                );

                await _deliveryAddressRepository.AddAsync(address);

                
                var auditLog = new AuditLog(
                    _authService.CurrentUser()!.Id,
                    "CreateDeliveryAddress",
                    nameof(DeliveryAddress),
                    address.Id,
                    $"Added delivery address at {address.Street}, {address.LgaId}/{address.StateId}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );

                await _auditLogRepository.AddAsync(auditLog);

              
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Delivery address {AddressId} created for customer {CustomerId}", address.Id, customer.Id);

                return Result<Guid>.Success(address.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating delivery address for customer {CustomerId}", request.Request.CustomerId);
                return Result<Guid>.Failure("An error occurred while creating delivery address");
            }
        }
    }

}
