using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersHandler(
     IMemoryCacheService distributedCache,
     IUserRepository userRepository)
     : IRequestHandler<GetAllUsersQuery, Result<PaginatedList<UserDto>>>
    {
        public async Task<Result<PaginatedList<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var key = $"AllUser:Page={request.PageRequest.Page}:Size={request.PageRequest.PageSize}";

            var users = await distributedCache.GetOrAddAsync(key,
                async () =>
                {
                    var result = await userRepository.GetAllAsync(request.PageRequest);

                    var dtos = result.Items.Select(u => u.UserAsDto()).ToList();

                    return new PaginatedList<UserDto>(
                        dtos,
                        result.TotalCount,
                        result.PageNumber,
                        result.PageSize
                    );
                });

            return Result<PaginatedList<UserDto>>.Success(users);
        }
    }

}
