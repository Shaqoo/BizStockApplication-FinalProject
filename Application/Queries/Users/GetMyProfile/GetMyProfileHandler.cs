using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Users.GetMyProfile
{
    public class GetMyProfileHandler(
     IAuthService authService,
     IUserRepository userRepository,
     IMemoryCacheService distributedCacheService)
     : IRequestHandler<GetMyProfileQuery, Result<UserDto>>
    {
        public async Task<Result<UserDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if (currentUser == null)
            {
                return Result<UserDto>.Failure("User not found.");
            }

            string cacheKey = $"UserProfile:{currentUser.Id}";

            var userDto = await distributedCacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var user = await userRepository.GetByIdAsync(currentUser.Id);
                    if (user == null)
                    {
                        return null!;
                    }
                    return user.UserAsDto();
                },
                TimeSpan.FromMinutes(10) 
            );

            if (userDto == null)
            {
                return Result<UserDto>.Failure("User not found.");
            }

            return Result<UserDto>.Success(userDto);
        }
    }

}
