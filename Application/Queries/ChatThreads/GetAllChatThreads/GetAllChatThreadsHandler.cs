using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatThreads.GetAllChatThreads
{
    public class GetAllChatThreadsQueryHandler(
     IChatThreadRepository repository,
     IMemoryCacheService cache)
     : IRequestHandler<GetAllChatThreadsQuery, Result<PaginatedList<ChatThreadDto>>>
    {
        public async Task<Result<PaginatedList<ChatThreadDto>>> Handle(GetAllChatThreadsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"chat-threads:page:{request.PageRequest.Page}:size:{request.PageRequest.PageSize}";

            var paginatedThreads = await cache.GetOrAddAsync(cacheKey, async () =>
            {
                var pagedThreads = await repository.GetAllAsync(request.PageRequest);

                var dtoList = pagedThreads.Items.Select(thread => thread.ChatThreadAsDto()).ToList();

                return new PaginatedList<ChatThreadDto>
                {
                    Items = dtoList,
                    TotalCount = pagedThreads.TotalCount,
                    PageNumber = pagedThreads.PageNumber,
                    PageSize = pagedThreads.PageSize
                };
            });

            return Result<PaginatedList<ChatThreadDto>>.Success(paginatedThreads);
        }

    }


}
