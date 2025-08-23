using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Carts.GetByUserId
{
    public record GetCartItemsByUserIdQuery(Guid UserId, PageRequest PageRequest)
    : IRequest<Result<PaginatedList<CartItemDto>>>;
}
