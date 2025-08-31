using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;


namespace Application.Commands.Carts.AddItem
{
    public record AddCartItemCommand(AddCartItemRequest AddCartItemRequest)
    : IRequest<Result<CartDto>>;
}
