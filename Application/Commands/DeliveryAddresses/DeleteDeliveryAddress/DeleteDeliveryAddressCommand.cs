using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.DeliveryAddresses.DeleteDeliveryAddress
{
    public record DeleteDeliveryAddressCommand(Guid Id,RequestMetadata RequestMetadata) : IRequest<Result<bool>>;
}
