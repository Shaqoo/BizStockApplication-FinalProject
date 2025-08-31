using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Carts.GetByCurrentUser
{
    public record GetCartByCurrentUserQuery(PageRequest PageRequest)
    : IRequest<Result<PaginatedCartDto>>;

}
