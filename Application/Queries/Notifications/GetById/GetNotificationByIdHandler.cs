using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Notifications.GetById
{
    public class GetNotificationByIdHandler
    : IRequestHandler<GetNotificationByIdQuery, Result<NotificationDto>>
    {
        private readonly INotificationRepository _repository;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<GetNotificationByIdHandler> _logger;

        public GetNotificationByIdHandler(
            INotificationRepository repository,
            IMemoryCacheService cache,
            ILogger<GetNotificationByIdHandler> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<NotificationDto>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"notifications:{request.Id}";

                var notification = await _cache.GetOrAddAsync(cacheKey, async () =>
                {
                    return await _repository.GetByIdAsync(request.Id);
                }, TimeSpan.FromMinutes(5));

                if (notification == null)
                    return Result<NotificationDto>.Failure("Notification not found.");

                return Result<NotificationDto>.Success(new NotificationDto 
                {
                    Message = notification.Message,
                    Timestamp = notification.DateCreated.DateTime,
                    Title = notification.Title,
                    Type = notification.Type
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notification with Id {NotificationId}", request.Id);
                return Result<NotificationDto>.Failure("Failed to retrieve notification.");
            }
        }
    }

}
