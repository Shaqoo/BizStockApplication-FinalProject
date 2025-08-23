namespace Application.Commands.Notifications.MarkAsRead
{
    using Application.Dto;
    using Application.Interfaces.Repository;
    using Application.Interfaces.Service;
    using Application.Interfaces.UnitOfWork;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class MarkNotificationAsReadCommandHandler
        : IRequestHandler<MarkNotificationAsReadCommand, Result<string>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<MarkNotificationAsReadCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IAuthService _authService;

        public MarkNotificationAsReadCommandHandler(
            INotificationRepository notificationRepository,
            INotifier notifier,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            ILogger<MarkNotificationAsReadCommandHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
            _notifier = notifier;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _authService.CurrentUser();
                if (user == null)
                {
                    _logger.LogWarning("Unauthorized attempt to mark notification {NotificationId} as read", request.NotificationId);
                    return Result<string>.Failure("Unauthorized");
                }
                await _notificationRepository.MarkAsReadAsync(request.NotificationId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Notification {NotificationId} marked as read", request.NotificationId);

                await _notifier.NotifyNotificationReadAsync(user.Id, request.NotificationId);

                return Result<string>.Success("Notification marked as read successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark notification {NotificationId} as read", request.NotificationId);
                return Result<string>.Failure("Error marking notification as read");
            }
        }
    }

}
