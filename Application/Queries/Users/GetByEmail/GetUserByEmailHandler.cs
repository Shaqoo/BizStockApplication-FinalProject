using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Users.GetByEmail
{
    public class GetUserByEmailHandler(
        IUserRepository userRepository,
        IMemoryCacheService cacheService)
        : IRequestHandler<GetUserByEmailQuery, Result<UserDto>>
    {
        public async Task<Result<UserDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetUserByEmail:{request.Email.ToLower()}";

            var userDto = await cacheService.GetOrAddAsync<UserDto>(
                cacheKey,
                async () =>
                {
                    var user = await userRepository.GetByEmailAsync(request.Email);
                    return user?.UserAsDto();
                },
                TimeSpan.FromMinutes(30)
            );

            if (userDto == null)
                return Result<UserDto>.Failure("User not found.");

            return Result<UserDto>.Success(userDto);
        }
    }

}
