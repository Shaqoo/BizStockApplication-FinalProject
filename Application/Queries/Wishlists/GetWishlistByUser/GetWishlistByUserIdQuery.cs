using Application.Dto;
using MediatR;

namespace Application.Queries.Wishlists.GetWishlistByUser
{
    public record GetWishlistByUserIdQuery : IRequest<Result<WishlistDto>>;
}
