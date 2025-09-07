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
        private readonly IAuthService _authService;

        public MarkAllNotificationsAsReadCommandHandler(
            INotificationRepository notificationRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            ILogger<MarkAllNotificationsAsReadCommandHandler> logger)
        {
            _notifier = notifier;
            _authService = authService;
            _notificationRepository = notificationRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var user = _authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("User Not Authenticated");
            try
            {
                await _notificationRepository.MarkAllAsReadAsync(user.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("All notifications for recipient {RecipientId} marked as read", user.Id);

                await _notifier.NotifyAllNotificationsReadAsync(user.Id);
                return Result<string>.Success("All notifications marked as read successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark all notifications for recipient {RecipientId} as read", user.Id);
                return Result<string>.Failure("Error marking all notifications as read");
            }
        }
    }

}
