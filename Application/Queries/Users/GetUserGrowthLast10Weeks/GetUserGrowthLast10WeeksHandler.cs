using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Users.GetUserGrowthLast10Weeks
{
    public class GetUserGrowthLast10WeeksHandler
    : IRequestHandler<GetUserGrowthLast10WeeksQuery, Result<UserGrowthFullDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCacheService _cache;

        public GetUserGrowthLast10WeeksHandler(
            IUserRepository userRepository,
            IMemoryCacheService cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task<Result<UserGrowthFullDto>> Handle(
            GetUserGrowthLast10WeeksQuery request,
            CancellationToken cancellationToken)
        {
            string cacheKey = "user_growth_last_10_weeks";

            var data = await _cache.GetOrAddAsync(
                cacheKey,
                async () => { 
                    var result = await _userRepository.GetUserGrowthLast10WeeksAsync();
                    var totalUsers = await _userRepository.CountAsync(a => a.UserRoles.Any(a => a.Role != Domain.Enums.Role.None));
                    return new UserGrowthFullDto
                    {
                        TotalUsers = totalUsers,
                        UserGrowthDtos = result
                    };
                } ,
                TimeSpan.FromMinutes(5) 
            );

            return Result<UserGrowthFullDto>.Success(data ?? new UserGrowthFullDto());
        }
    }

}
