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

namespace Application.Queries.ChatThreads.GetByStatus
{
    public class GetChatThreadsByStatusQueryHandler(
    IChatThreadRepository repository,
    IMemoryCacheService cache)
    : IRequestHandler<GetChatThreadsByStatusQuery, Result<PaginatedList<ChatThreadDto>>>
    {
        public async Task<Result<PaginatedList<ChatThreadDto>>> Handle(GetChatThreadsByStatusQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"chat-threads:status:{request.Status}:page:{request.PageRequest.Page}:size:{request.PageRequest.PageSize}";

            var result = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var paged = await repository.GetByStatusAsync(request.Status, request.PageRequest);

                var dtoList = paged.Items.Select(thread => thread.ChatThreadAsDto()).ToList();

                return new PaginatedList<ChatThreadDto>
                {
                    Items = dtoList,
                    TotalCount = paged.TotalCount,
                    PageNumber = paged.PageNumber,
                    PageSize = paged.PageSize
                };
            }, TimeSpan.FromMinutes(1));

            return Result<PaginatedList<ChatThreadDto>>.Success(result);
        }
    }

}
