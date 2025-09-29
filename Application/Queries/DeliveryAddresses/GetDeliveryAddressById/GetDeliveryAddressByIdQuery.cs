using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAddresses.GetDeliveryAddressById
{
    public record GetDeliveryAddressByIdQuery(Guid AddressId)
    : IRequest<Result<DeliveryAddressDto>>;
}
