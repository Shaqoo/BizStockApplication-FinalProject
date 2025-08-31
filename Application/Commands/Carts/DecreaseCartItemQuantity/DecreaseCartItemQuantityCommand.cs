using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Carts.DecreaseCartItemQuantity
{
    public record DecreaseCartItemQuantityCommand(DecreaseCartItemQuantityRequest Request)
       : IRequest<Result<CartDto>>;
}
