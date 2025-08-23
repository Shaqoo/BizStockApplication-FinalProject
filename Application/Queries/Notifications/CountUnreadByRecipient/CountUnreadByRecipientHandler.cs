using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Notifications.CountUnreadByRecipient
{
    public class CountUnreadByRecipientHandler : IRequestHandler<CountUnreadByRecipientQuery, Result<int>>
    {
        private readonly INotificationRepository _repository;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<CountUnreadByRecipientHandler> _logger;

        public CountUnreadByRecipientHandler(
            INotificationRepository repository,
            IMemoryCacheService cache,
            ILogger<CountUnreadByRecipientHandler> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(CountUnreadByRecipientQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"notifications:unreadcount:{request.RecipientId}";

                var count = await _cache.GetOrAddAsync(cacheKey, async () =>
                {
                    return await _repository.CountUnreadByRecipientAsync(request.RecipientId);
                }, TimeSpan.FromMinutes(10)); 

                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting unread notifications for recipient {RecipientId}", request.RecipientId);
                return Result<int>.Failure("Failed to count unread notifications.");
            }
        }
    }

}
