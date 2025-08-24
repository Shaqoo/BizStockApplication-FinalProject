using Application.Dto;
using Application.Pagination;
using Domain.Enums;
using MediatR;

namespace Application.Queries.Users.GetByRole
{
    public record GetUsersByRoleQuery(Role Role,PageRequest PageRequest) : IRequest<Result<PaginatedList<UserDto>>>;
}
