using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Users.SearchBykeyword
{
    public record SearchUsersByKeywordQuery(PageRequest PageRequest,string keyword) : IRequest<Result<PaginatedList<UserDto>>>;
     
}
