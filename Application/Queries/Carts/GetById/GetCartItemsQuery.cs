using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Carts.GetById
{
    public record GetCartItemsQuery(Guid CartId, PageRequest PageRequest)
    : IRequest<Result<PaginatedList<CartItemDto>>>;
}
