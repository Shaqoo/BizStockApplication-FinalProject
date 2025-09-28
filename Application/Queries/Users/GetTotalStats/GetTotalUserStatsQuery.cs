using Application.Dto;
using MediatR;

namespace Application.Queries.Users.GetTotalStats
{
    public record GetTotalUserStatsQuery : IRequest<Result<TotalUserStatsDto>>;
   
}
