using Application.Dto;
using Application.Pagination;
using MediatR;
namespace Application.Queries.Wishlists.GetItemsByUser
{
    public record GetWishlistItemsByUserQuery(PageRequest PageRequest) : IRequest<Result<PaginatedList<WishlistItemDto>>>;
}
