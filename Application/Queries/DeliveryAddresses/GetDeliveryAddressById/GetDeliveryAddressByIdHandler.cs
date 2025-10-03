using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.DeliveryAddresses.GetDeliveryAddressById
{
    public class GetDeliveryAddressByIdQueryHandler
    : IRequestHandler<GetDeliveryAddressByIdQuery, Result<DeliveryAddressDto>>
    {
        private readonly IDeliveryAddressRepository deliveryAddressRepository;
        private readonly ILogger<GetDeliveryAddressByIdQueryHandler> logger;
        private readonly IMemoryCacheService cacheService;

        public GetDeliveryAddressByIdQueryHandler(
            IDeliveryAddressRepository deliveryAddressRepository,
            ILogger<GetDeliveryAddressByIdQueryHandler> logger,
            IMemoryCacheService cacheService)
        {
            this.deliveryAddressRepository = deliveryAddressRepository;
            this.logger = logger;
            this.cacheService = cacheService;
        }

        public async Task<Result<DeliveryAddressDto>> Handle(GetDeliveryAddressByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"delivery_address_{request.AddressId}";

                var address = await cacheService.GetOrAddAsync(
                    cacheKey,
                    async () =>
                    {
                        var address = await deliveryAddressRepository.GetByIdAsync(request.AddressId);
                        if (address == null)
                        {
                            return null;
                        }
                        return new DeliveryAddressDto
                        {
                            CustomerId = address.CustomerId,
                            IsDefault = address.IsDefault,
                            CreatedAt = address.CreatedAt,
                            Id = address.Id,
                            Landmark = address.Landmark,
                            LgaId = address.LgaId,
                            LgaName = address.Lga.Name,
                            PostalCode = address.PostalCode,
                            StateId = address.StateId,
                            StateName = address.State.Name,
                            Street = address.Street,
                            PhoneNumber = address.PhoneNumber,
                            AdditionalPhoneNumber = address.AdditionalPhoneNumber,
                            CustomerName = address.FullName,
                            Email = address.Email
                        };
                    },
                    TimeSpan.FromMinutes(1)
                );

                if (address is null)
                {
                    logger.LogWarning("Delivery address {AddressId} not found", request.AddressId);
                    return Result<DeliveryAddressDto>.Failure("Delivery address not found");
                }

                return Result<DeliveryAddressDto>.Success(address);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving delivery address {AddressId}", request.AddressId);
                return Result<DeliveryAddressDto>.Failure("An error occurred while retrieving the delivery address");
            }
        }
    }

}
