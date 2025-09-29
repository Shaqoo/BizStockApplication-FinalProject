using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.DeliveryAddresses.SetDefaultDeliveryAddress
{
    public class SetDefaultDeliveryAddressCommandHandler
    : IRequestHandler<SetDefaultDeliveryAddressCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<SetDefaultDeliveryAddressCommandHandler> logger;
        private readonly ICustomerRepository customerRepository;

        public SetDefaultDeliveryAddressCommandHandler(
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            ILogger<SetDefaultDeliveryAddressCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.customerRepository = customerRepository;
        }

        public async Task<Result<bool>> Handle(SetDefaultDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var customer = await customerRepository.GetByIdAsync(request.CustomerId);
                if (customer is null)
                {
                    logger.LogWarning("Customer {CustomerId} not found", request.CustomerId);
                    return Result<bool>.Failure("Customer not found");
                }

                var address = customer.DeliveryAddresses.FirstOrDefault(a => a.Id == request.AddressId);
                if (address is null)
                {
                    logger.LogWarning("Delivery address {AddressId} not found for customer {CustomerId}",
                        request.AddressId, request.CustomerId);
                    return Result<bool>.Failure("Delivery address not found");
                }

               
                customer.ClearDefaultDeliveryAddresses();

                 
                address.SetDefault(true);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Customer {CustomerId} set address {AddressId} as default",
                    request.CustomerId, request.AddressId);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting default delivery address {AddressId} for customer {CustomerId}",
                    request.AddressId, request.CustomerId);
                return Result<bool>.Failure("An error occurred while setting the default delivery address");
            }
        }
    }

}
