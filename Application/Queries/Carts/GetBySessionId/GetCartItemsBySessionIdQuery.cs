using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Carts.GetBySessionId
{
    public record GetCartItemsBySessionIdQuery(string SessionId, PageRequest PageRequest)
    : IRequest<Result<PaginatedList<CartItemDto>>>;
}
