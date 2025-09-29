using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAddresses.GetDeliveryAddressesByCustomer
{
    public record GetDeliveryAddressesByCustomerQuery(Guid CustomerId)
    : IRequest<Result<IEnumerable<DeliveryAddressDto>>>;
}
