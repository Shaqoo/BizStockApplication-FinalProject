using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.DeliveryAddresses.AddDeliveryAddress
{

    public record CreateDeliveryAddressCommand(CreateDeliveryAddressRequest Request,RequestMetadata RequestMetadata)
        : IRequest<Result<Guid>>;

}
