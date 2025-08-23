
using Application.Dto;
using MediatR;

namespace Application.Commands.Wishlists.CreateWishlist
{
    public record CreateWishlistCommand(Guid UserId) : IRequest<Result<string>>;
}
