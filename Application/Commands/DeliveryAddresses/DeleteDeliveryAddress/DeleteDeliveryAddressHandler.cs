using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.DeliveryAddresses.DeleteDeliveryAddress
{
    public class DeleteDeliveryAddressCommandHandler
    : IRequestHandler<DeleteDeliveryAddressCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<DeleteDeliveryAddressCommandHandler> logger;
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly IAuthService _authService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public DeleteDeliveryAddressCommandHandler(
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IAuthService authService,
            IDeliveryAddressRepository deliveryAddressRepository,
            ILogger<DeleteDeliveryAddressCommandHandler> logger)
        {
            _auditLogRepository = auditLogRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            _authService = authService;
            _customerRepository = customerRepository;
            _deliveryAddressRepository = deliveryAddressRepository;
        }

        public async Task<Result<bool>> Handle(DeleteDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userEmail = _authService.CurrentUser();
            if (userEmail == null)
            {
                logger.LogWarning("User Not Authenticated");
                return Result<bool>.Failure("User Not Found");
            }
            try
            {
                var customer = await _customerRepository.GetByEmailAsync(userEmail.Email);
                if (customer == null)
                {
                    logger.LogWarning("Customer Not Found");
                    return Result<bool>.Failure("Customer not found");
                }
                var deliveryAddress = await _deliveryAddressRepository.GetByIdAsync(request.Id);
                if (deliveryAddress is null || deliveryAddress.CustomerId != customer.Id)
                {
                    logger.LogWarning("Delivery address not found for customer {CustomerId}", customer.Id);
                    return Result<bool>.Failure("Delivery address not found");
                }

                await _deliveryAddressRepository.DeleteAsync(deliveryAddress.Id);

               
                var auditLog = new AuditLog(
                    userEmail.Id,
                    "DELETE_DELIVERY_ADDRESS",
                    nameof(DeliveryAddress),
                    request.Id,
                    $"Deleted delivery address for customer {(string)customer.Email}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );

                await _auditLogRepository.AddAsync(auditLog);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Delivery address {DeliveryAddressId} deleted for customer {CustomerId}",
                    request.Id, userEmail.Email);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting delivery address {DeliveryAddressId} for customer {CustomerId}",
                    request.Id, userEmail.Email);
                return Result<bool>.Failure("An error occurred while deleting the delivery address");
            }
        }
    }

}
