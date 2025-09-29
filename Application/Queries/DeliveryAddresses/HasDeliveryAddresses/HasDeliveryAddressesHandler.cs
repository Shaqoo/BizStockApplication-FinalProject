using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.DeliveryAddresses.HasDeliveryAddresses
{
    public class HasDeliveryAddressesHandler(ICustomerRepository customerRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<HasDeliveryAddressesQuery, Result<bool>>
    {
        public async Task<Result<bool>> Handle(HasDeliveryAddressesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"HasDeliveryAddressesQuery:{request.CustomerId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var customer = await customerRepository.GetByIdAsync(request.CustomerId);
                    if (customer == null)
                        return false;
                    return customer.HasDefaultDeliveryAddress();
                },TimeSpan.FromMinutes(5));
            return Result<bool>.Success(cachedResult);
        }
    }
}
