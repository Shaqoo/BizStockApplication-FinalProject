using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Notifications.SendNotificationToUser
{
    public class SendNotificationToUserHandler
    : IRequestHandler<SendNotificationToUserCommand, Result<string>>
    {
        private readonly INotificationRepository _repository;
        private readonly ILogger<SendNotificationToUserHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly INotifier _notifier;
        public SendNotificationToUserHandler(
            INotificationRepository repository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            INotifier notifier,
            ILogger<SendNotificationToUserHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _notifier = notifier;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(SendNotificationToUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.Request.UserId ?? Guid.Empty);
                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found. Notification not sent.", request.Request.UserId);
                    return Result<string>.Failure("User not found. Notification not sent.");
                }
                var notification = new Notification(user.Id,request.Request.Title,request.Request.Message,
               request.Request.Type);

                await _repository.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _notifier.SendNotificationAsync(user.Id,new NotificationDto 
                {
                    Id = notification.Id,
                    Message = notification.Message,
                    Timestamp = notification.DateCreated.DateTime,
                    Title = notification.Title,
                    Type = notification.Type
                });

                _logger.LogInformation("Notification sent to User {UserId}", request.Request.UserId);

                return Result<string>.Success("Notification sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification to User {UserId}", request.Request.UserId);
                return Result<string>.Failure("Failed to send notification.");
            }
        }
    }

}
