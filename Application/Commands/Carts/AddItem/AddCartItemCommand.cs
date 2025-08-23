using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;


namespace Application.Commands.Carts.AddItem
{
    public record AddCartItemCommand(AddCartItemRequest CartItemRequest) : IRequest<Result<CartItemDto>>;

}
