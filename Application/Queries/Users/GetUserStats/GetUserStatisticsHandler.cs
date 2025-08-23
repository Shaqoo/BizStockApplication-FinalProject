using Application.Dto;
using Application.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Users.GetUserStats
{
    public class GetUserStatisticsHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserStatisticsQuery, Result<UserStatisticsDto>>
    {
        public async Task<Result<UserStatisticsDto>> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
        {
            var totalUsers = await userRepository.CountAsync();
            var activeUsers = await userRepository.CountAsync(u => !u.IsDeleted);
            var inactiveUsers = await userRepository.CountAsync(u => u.IsDeleted);
            var newUsersLast7Days = await userRepository.CountAsync(u => u.DateCreated >= DateTimeOffset.UtcNow.AddDays(-7));
            var newUsersToday = await userRepository.CountAsync(u => u.DateCreated == DateTimeOffset.UtcNow.Date);

            var dto = new UserStatisticsDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                NewUsersLast7Days = newUsersLast7Days,
                NewUsersToday = newUsersToday
            };

            return Result<UserStatisticsDto>.Success(dto);
        }
    }

}
