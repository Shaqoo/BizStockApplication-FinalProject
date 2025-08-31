using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Carts.GetByUserId
{
    public record GetCartByUserIdQuery(Guid UserId, PageRequest PageRequest)
     : IRequest<Result<PaginatedCartDto>>;

}
