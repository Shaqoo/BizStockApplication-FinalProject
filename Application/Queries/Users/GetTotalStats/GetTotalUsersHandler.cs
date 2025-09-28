using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Users.GetTotalStats
{
    public class GetTotalUsersHandler(IUserRepository userRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetTotalUserStatsQuery, Result<TotalUserStatsDto>>
    {
        public async Task<Result<TotalUserStatsDto>> Handle(GetTotalUserStatsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "GetTotalUserStatsQuery";

            var cahedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var result = await userRepository.GetTotalUserStats();
                    return result;
                },TimeSpan.FromMinutes(5));

            return Result<TotalUserStatsDto>.Success(cahedResult ?? new TotalUserStatsDto());
        }
    }
}
