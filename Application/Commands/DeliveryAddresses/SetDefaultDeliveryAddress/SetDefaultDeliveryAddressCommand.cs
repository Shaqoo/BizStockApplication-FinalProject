using Application.Dto;
using MediatR;

namespace Application.Commands.DeliveryAddresses.SetDefaultDeliveryAddress
{
    public record SetDefaultDeliveryAddressCommand(Guid CustomerId, Guid AddressId) : IRequest<Result<bool>>;
}
