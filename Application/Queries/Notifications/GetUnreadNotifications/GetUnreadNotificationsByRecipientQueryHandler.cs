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

        public GetUnreadNotificationsByRecipientQueryHandler(
            INotificationRepository notificationRepository,
            IMemoryCacheService cacheService,
            ILogger<GetUnreadNotificationsByRecipientQueryHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<NotificationDto>>> Handle(GetUnreadNotificationsByRecipientQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"notifications_unread_{request.RecipientId}";

            var result = await _cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                try
                {
                    var notifications = await _notificationRepository.GetUnreadByRecipientAsync(request.RecipientId);

                    var result = notifications.Select(n => new NotificationDto
                    {
                        Title = n.Title,
                        Message = n.Message,
                        Type = n.Type,
                        Timestamp = n.DateCreated.DateTime
                    });

                    return Result<IEnumerable<NotificationDto>>.Success(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get unread notifications for recipient {RecipientId}", request.RecipientId);
                    return Result<IEnumerable<NotificationDto>>.Failure("Error fetching unread notifications");
                }
            },TimeSpan.FromMinutes(10));
            return result;
        }
    }

}
