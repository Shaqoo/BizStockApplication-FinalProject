using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.DeliveryAddresses.UpdateDeliveryAddress
{
    public record UpdateDeliveryAddressCommand(UpdateDeliveryAddressRequest Request,RequestMetadata RequestMetadata)
        : IRequest<Result<bool>>;
}
