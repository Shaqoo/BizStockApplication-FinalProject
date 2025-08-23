using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Carts.Update
{
    public record UpdateCartItemQuantityCommand(UpdateCartItemQuantityRequest UpdateCartItem) : IRequest<Result<string>>;

}
