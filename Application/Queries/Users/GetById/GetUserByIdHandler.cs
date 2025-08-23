using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Users.GetById
{
    public class GetUserByIdHandler(
     IUserRepository userRepository,
     IMemoryCacheService distributedCache)
     : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetUserById:{request.id}";

            var userDto = await distributedCache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var user = await userRepository.GetByIdAsync(request.id);
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
