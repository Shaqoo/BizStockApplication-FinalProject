using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.ChatThreads.GetStats
{
    public class GetChatThreadStatsHandler(IChatThreadRepository chatThreadRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetChatThreadStatsQuery, Result<ChatThreadStatsDto>>
    {
        public async Task<Result<ChatThreadStatsDto>> Handle(GetChatThreadStatsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "chat_thread_stats";

            var cachedStats = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var inProgressThreads = await chatThreadRepository.CountInprogressThreadsAsync();
                var openThreads = await chatThreadRepository.CountOpenThreadsAsync();
                var closedThreads = await chatThreadRepository.CountClosedThreadsAsync();
                return new ChatThreadStatsDto
                {
                    TotalThreads = inProgressThreads + openThreads + closedThreads,
                    InProgressThreads = inProgressThreads,
                    OpenThreads = openThreads,
                    ClosedThreads = closedThreads
                };
            }, TimeSpan.FromMinutes(1));

            return Result<ChatThreadStatsDto>.Success(cachedStats);
        }
    }
}
