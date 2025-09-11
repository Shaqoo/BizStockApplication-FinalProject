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

namespace Application.Queries.ChatMessages.GetMessagesByThread
{
    public class GetMessagesByThreadIdHandler(
    IChatMessageRepository messageRepository,
    IMemoryCacheService cacheService)
    : IRequestHandler<GetMessagesByThreadIdQuery, Result<PaginatedList<MessageDto>>>
    {
        public async Task<Result<PaginatedList<MessageDto>>> Handle(GetMessagesByThreadIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"chat-thread-messages:{request.ChatThreadId}:page:{request.PageRequest.Page}:{request.PageRequest.PageSize}";

            var pagedMessages = await cacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var paginated = await messageRepository.GetMessagesByThreadIdPagedAsync(
                        request.ChatThreadId,
                        request.PageRequest);

                    var dtoList = paginated.Items
                        .Select(m => m.AsDto())
                        .ToList();

                    return new PaginatedList<MessageDto>(
                        dtoList,
                        paginated.TotalCount,
                        paginated.PageNumber,
                        paginated.PageSize
                    );
                },
                TimeSpan.FromMinutes(1) 
            );

            return Result<PaginatedList<MessageDto>>.Success(pagedMessages);
        }
    }

}
