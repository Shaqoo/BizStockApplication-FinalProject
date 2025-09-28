using Application.Dto;
using MediatR;

namespace Application.Queries.Users.GetUserGrowthLast10Weeks
{
    public record GetUserGrowthLast10WeeksQuery : IRequest<Result<UserGrowthFullDto>>;

}
