using Application.Dto;
using MediatR;

namespace Application.Queries.Wishlists.GetWishlistById
{
    public record GetWishlistByIdQuery(Guid Id) : IRequest<Result<WishlistDto>>;
}
