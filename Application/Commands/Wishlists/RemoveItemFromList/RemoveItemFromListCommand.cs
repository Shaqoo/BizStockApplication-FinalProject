using Application.Dto;
using MediatR;

namespace Application.Commands.Wishlists.RemoveItemFromList
{
    public record RemoveItemFromListCommand(Guid ProductId) : IRequest<Result<string>>;
}
