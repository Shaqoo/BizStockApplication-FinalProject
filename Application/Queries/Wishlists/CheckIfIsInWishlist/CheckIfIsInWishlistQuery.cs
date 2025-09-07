using Application.Dto;
using MediatR;

namespace Application.Queries.Wishlists.CheckIfIsInWishlist
{
    public record CheckIfIsInWishlistQuery(Guid ProductId) : IRequest<Result<bool>>;
}
