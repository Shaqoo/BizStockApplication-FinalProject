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

namespace Application.Queries.ChatThreads.GetChatThreadsByAgent
{
    public class GetChatThreadsByAssignedAgentQueryHandler(
    IChatThreadRepository repository,
    IAuthService authService,
    IMemoryCacheService cache)
    : IRequestHandler<GetChatThreadsByAssignedAgentQuery, Result<PaginatedList<ChatThreadDto>>>
    {
        public async Task<Result<PaginatedList<ChatThreadDto>>> Handle(GetChatThreadsByAssignedAgentQuery request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if(currentUser == null)
            {
                return Result<PaginatedList<ChatThreadDto>>.Failure("User not authenticated.");
            }
            if (currentUser.RoleName != "CustomerService")
            {
                return Result<PaginatedList<ChatThreadDto>>.Failure("User is not a CustomerService.");
            }

            var cacheKey = $"chat-threads:agent:{currentUser.Id}:page:{request.PageRequest.Page}:size:{request.PageRequest.PageSize}";

            var result = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var paged = await repository.GetByAgentIdAsync(currentUser.Id, request.PageRequest);

                var dtoList = paged.Items.Select(thread => thread.ChatThreadAsDto()).ToList();

                return new PaginatedList<ChatThreadDto>
                {
                    Items = dtoList,
                    TotalCount = paged.TotalCount,
                    PageNumber = paged.PageNumber,
                    PageSize = paged.PageSize
                };
            },TimeSpan.FromMinutes(1));

            return Result<PaginatedList<ChatThreadDto>>.Success(result);
        }
    }

}
