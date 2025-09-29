using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAddresses.HasDeliveryAddresses
{
    public record HasDeliveryAddressesQuery(Guid CustomerId)
    : IRequest<Result<bool>>;
}
