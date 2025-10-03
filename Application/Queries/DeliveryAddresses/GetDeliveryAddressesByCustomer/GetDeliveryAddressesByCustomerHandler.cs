using global::Application.Dto;
using global::Application.Interfaces.Repository;
using global::Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.DeliveryAddresses.GetDeliveryAddressesByCustomer
{
    namespace Application.DeliveryAddresses.Queries.Handlers
    {
        public class GetDeliveryAddressesByCustomerQueryHandler
            : IRequestHandler<GetDeliveryAddressesByCustomerQuery, Result<IEnumerable<DeliveryAddressDto>>>
        {
            private readonly IDeliveryAddressRepository _repository;
            private readonly IMemoryCacheService _cacheService;

            public GetDeliveryAddressesByCustomerQueryHandler(
                IDeliveryAddressRepository repository,
                IMemoryCacheService cacheService)
            {
                _repository = repository;
                _cacheService = cacheService;
            }

            public async Task<Result<IEnumerable<DeliveryAddressDto>>> Handle(
                GetDeliveryAddressesByCustomerQuery request,
                CancellationToken cancellationToken)
            {
                string cacheKey = $"delivery_addresses_customer_{request.CustomerId}";

                var addresses = await _cacheService.GetOrAddAsync(
                    cacheKey,
                    async () =>
                    {
                        var entities = await _repository.GetByCustomerIdAsync(request.CustomerId);
                        return entities.Select(a => new DeliveryAddressDto
                        {
                            Id = a.Id,
                            CustomerId = a.CustomerId,
                            Street = a.Street,
                            LgaId = a.LgaId,
                            LgaName = a.Lga.Name,
                            StateId = a.StateId,
                            CreatedAt = a.CreatedAt,
                            Landmark = a.Landmark,
                            StateName = a.State.Name,
                            PostalCode = a.PostalCode,
                            IsDefault = a.IsDefault,
                            PhoneNumber = a.PhoneNumber,
                            Email = a.Email,
                            CustomerName = a.FullName,
                            AdditionalPhoneNumber = a.AdditionalPhoneNumber
                        }).ToList();
                    },TimeSpan.FromMinutes(1));

                return Result<IEnumerable<DeliveryAddressDto>>.Success(addresses ?? []);
            }
        }
    }

}
