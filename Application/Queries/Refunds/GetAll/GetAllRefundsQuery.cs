using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Refunds.GetAll
{
    public record GetAllRefundsQuery(PageRequest PageRequest) : IRequest<Result<PaginatedList<RefundDto>>>;
    

}
