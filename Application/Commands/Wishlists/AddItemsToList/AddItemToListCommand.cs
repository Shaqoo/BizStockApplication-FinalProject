using Application.Dto;
using MediatR;

namespace Application.Commands.Wishlists.AddItemsToList
{
    public record AddItemToListCommand(Guid ProductId) : IRequest<Result<string>>;
}
