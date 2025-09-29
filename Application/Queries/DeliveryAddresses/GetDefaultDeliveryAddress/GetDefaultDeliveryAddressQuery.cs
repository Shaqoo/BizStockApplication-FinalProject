using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAddresses.GetDefaultDeliveryAddress
{
    public record GetDefaultDeliveryAddressQuery(Guid CustomerId)
    : IRequest<Result<DeliveryAddressDto>>;
}
