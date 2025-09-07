using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Notifications.GetUnreadNotifications
{
    public class GetUnreadNotificationsByRecipientQueryHandler
        : IRequestHandler<GetUnreadNotificationsByRecipientQuery, Result<IEnumerable<NotificationDto>>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMemoryCacheService _cacheService;
        private readonly ILogger<GetUnreadNotificationsByRecipientQueryHandler> _logger;
        private readonly IAuthService _authService;

        public GetUnreadNotificationsByRecipientQueryHandler(
            INotificationRepository notificationRepository,
            IAuthService authService,
            IMemoryCacheService cacheService,
            ILogger<GetUnreadNotificationsByRecipientQueryHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _authService = authService;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<NotificationDto>>> Handle(GetUnreadNotificationsByRecipientQuery request, CancellationToken cancellationToken)
        {
            var user = _authService.CurrentUser();
            if (user == null)
                return Result<IEnumerable<NotificationDto>>.Failure("User Not Authenticated");
            string cacheKey = $"notifications_unread_{user.Id}";

            var result = await _cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                try
                {
                    var notifications = await _notificationRepository.GetUnreadByRecipientAsync(user.Id);

                    var result = notifications.Select(n => new NotificationDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        Type = n.Type,
                        Timestamp = n.DateCreated.DateTime,
                        IsRead = n.IsRead,
                    });

                    return Result<IEnumerable<NotificationDto>>.Success(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get unread notifications for recipient {RecipientId}", user.Id);
                    return Result<IEnumerable<NotificationDto>>.Failure("Error fetching unread notifications");
                }
            },TimeSpan.FromMinutes(10));
            return result;
        }
    }

}
