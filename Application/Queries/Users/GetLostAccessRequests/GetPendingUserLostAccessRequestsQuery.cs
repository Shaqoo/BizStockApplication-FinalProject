using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Users.GetLostAccessRequests
{
    public record GetPendingUserLostAccessRequestsQuery(PageRequest PageRequest) 
        : IRequest<Result<PaginatedList<LostAccessRequestDto>>>;
     
}
