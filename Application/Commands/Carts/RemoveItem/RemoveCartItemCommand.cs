using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Carts.RemoveItem
{
    public record RemoveCartItemCommand(RemoveCartItemRequest RemoveCartItemRequest) : IRequest<Result<string>>;

}
