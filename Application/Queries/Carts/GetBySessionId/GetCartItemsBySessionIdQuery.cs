using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Carts.GetBySessionId
{
    public record GetCartBySessionIdQuery(string SessionId, PageRequest PageRequest)
    : IRequest<Result<PaginatedCartDto>>;

}
