using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Notifications.MarkAllAsRead
{
    public class MarkAllNotificationsAsReadCommandHandler
        : IRequestHandler<MarkAllNotificationsAsReadCommand, Result<string>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<MarkAllNotificationsAsReadCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;

        public MarkAllNotificationsAsReadCommandHandler(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            ILogger<MarkAllNotificationsAsReadCommandHandler> logger)
        {
            _notifier = notifier;
            _notificationRepository = notificationRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _notificationRepository.MarkAllAsReadAsync(request.RecipientId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("All notifications for recipient {RecipientId} marked as read", request.RecipientId);

                await _notifier.NotifyAllNotificationsReadAsync(request.RecipientId);
                return Result<string>.Success("All notifications marked as read successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark all notifications for recipient {RecipientId} as read", request.RecipientId);
                return Result<string>.Failure("Error marking all notifications as read");
            }
        }
    }

}
