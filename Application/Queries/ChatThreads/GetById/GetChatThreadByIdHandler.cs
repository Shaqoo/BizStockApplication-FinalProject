using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatThreads.GetById
{
    public class GetChatThreadByIdQueryHandler : IRequestHandler<GetChatThreadByIdQuery, Result<ChatThreadDto>>
    {
        private readonly IChatThreadRepository _repository;
        private readonly IMemoryCacheService _cache;

        public GetChatThreadByIdQueryHandler(IChatThreadRepository repository, IMemoryCacheService distributedCacheService)
        {
            _repository = repository;
            _cache = distributedCacheService;
        }

        public async Task<Result<ChatThreadDto>> Handle(GetChatThreadByIdQuery request, CancellationToken cancellationToken)
        {
            var dtoResult = await _cache.GetOrAddAsync(
            $"chat-thread:{request.ThreadId}",
            async () =>
            {
                var thread = await _repository.GetByIdAsync(request.ThreadId);
                if (thread is null)
                    return null;

                return thread.ChatThreadAsDto();
            });

            if (dtoResult is null)
                return Result<ChatThreadDto>.Failure("Chat thread not found");

            return Result<ChatThreadDto>.Success(dtoResult);

        }
    }

}
