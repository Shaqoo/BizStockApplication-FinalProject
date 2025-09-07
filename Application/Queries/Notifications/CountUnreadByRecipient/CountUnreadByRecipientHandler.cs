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
        private readonly IAuthService _authService;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<CountUnreadByRecipientHandler> _logger;

        public CountUnreadByRecipientHandler(
            INotificationRepository repository,
            IAuthService authService,
            IMemoryCacheService cache,
            ILogger<CountUnreadByRecipientHandler> logger)
        {
            _repository = repository;
            _authService = authService;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(CountUnreadByRecipientQuery request, CancellationToken cancellationToken)
        {
            var user = _authService.CurrentUser();
            if (user == null)
                return Result<int>.Failure("User Not Authenticated");
            try
            {
                var cacheKey = $"notifications:unreadcount:{user.Id}";

                var count = await _cache.GetOrAddAsync(cacheKey, async () =>
                {
                    return await _repository.CountUnreadByRecipientAsync(user.Id);
                }, TimeSpan.FromMinutes(1)); 

                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting unread notifications for recipient {RecipientId}", user.Id);
                return Result<int>.Failure("Failed to count unread notifications.");
            }
        }
    }

}
