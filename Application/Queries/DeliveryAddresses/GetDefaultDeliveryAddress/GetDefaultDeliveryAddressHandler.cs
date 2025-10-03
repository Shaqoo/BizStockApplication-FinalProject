using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.DeliveryAddresses.GetDefaultDeliveryAddress
{
    public class GetDefaultDeliveryAddressHandler(IMemoryCacheService memoryCacheService,
        ICustomerRepository customerRepository) : IRequestHandler<GetDefaultDeliveryAddressQuery, Result<DeliveryAddressDto>>
    {
        public async Task<Result<DeliveryAddressDto>> Handle(GetDefaultDeliveryAddressQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetDefaultDeliveryAddressQuery:{request.CustomerId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var customer = await customerRepository.GetByIdAsync(request.CustomerId);
                    if (customer == null)
                    {
                        return null;
                    }
                    var defaultAddress = customer.DeliveryAddresses.FirstOrDefault(a => a.IsDefault == true);
                    if (defaultAddress == null)
                    {
                        return null;
                    }
                    return new DeliveryAddressDto
                    {
                        CustomerId = defaultAddress.CustomerId,
                        IsDefault = defaultAddress.IsDefault,
                        CreatedAt = defaultAddress.CreatedAt,
                        Id = defaultAddress.Id,
                        Landmark = defaultAddress.Landmark,
                        LgaId = defaultAddress.LgaId,
                        LgaName = defaultAddress.Lga.Name,
                        PostalCode = defaultAddress.PostalCode,
                        StateId = defaultAddress.StateId,
                        StateName = defaultAddress.State.Name,
                        Street = defaultAddress.Street,
                        AdditionalPhoneNumber = defaultAddress.AdditionalPhoneNumber,
                        CustomerName = defaultAddress.FullName,
                        Email = defaultAddress.Email,
                        PhoneNumber = defaultAddress.PhoneNumber

                    };
                },TimeSpan.FromMinutes(1));

            if (cachedResult != null)
                return Result<DeliveryAddressDto>.Success(cachedResult);
            return Result<DeliveryAddressDto>.Failure("Default Address Not Found");
        }
    }
}
