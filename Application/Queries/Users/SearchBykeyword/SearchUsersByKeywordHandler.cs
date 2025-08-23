using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Users.SearchBykeyword
{
    public class SearchUsersByKeywordHandler(
    IMemoryCacheService distributedCache,
    IUserRepository userRepository)
    : IRequestHandler<SearchUsersByKeywordQuery, Result<PaginatedList<UserDto>>>
    {
        public async Task<Result<PaginatedList<UserDto>>> Handle(SearchUsersByKeywordQuery request, CancellationToken cancellationToken)
        {
            var key = $"SearchUser:{request.keyword}:Page={request.PageRequest.Page}:Size={request.PageRequest.PageSize}";

            var users = await distributedCache.GetOrAddAsync(key, async () =>
            {
                var result = await userRepository.SearchUsers(request.keyword, request.PageRequest);
                
                return new PaginatedList<UserDto>(
                    result.Items.Select(u => u.UserAsDto()).ToList(),
                    result.TotalCount,
                    result.PageNumber,
                    result.PageSize
                );
            });

            return Result<PaginatedList<UserDto>>.Success(users);
        }
    }

}
