using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Users.GetByRole
{
    public class GetUsersByRoleHandler(IMemoryCacheService memoryCacheService,
        IUserRepository userRepository) : IRequestHandler<GetUsersByRoleQuery, Result<PaginatedList<UserDto>>>
    {
        public async Task<Result<PaginatedList<UserDto>>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetUsersByRoleQuery:{request.Role}:Page{request.PageRequest.Page}:PageSize:{request.PageRequest.PageSize}";

            var response = await memoryCacheService.GetOrAddAsync(cacheKey,
                async() =>
                {
                    var result = await userRepository.GetUsersByRoleAsync(request.Role, request.PageRequest);
                    return result;
                },TimeSpan.FromMinutes(10));

            return Result<PaginatedList<UserDto>>.Success(response);
        }
    }
}
