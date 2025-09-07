using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Notifications.SendNotificationViaRoles
{
    public class SendNotificationToRoleHandler
    : IRequestHandler<SendNotificationToRoleCommand, Result<string>>
    {
        private readonly INotificationRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly ILogger<SendNotificationToRoleHandler> _logger;

        public SendNotificationToRoleHandler(
            INotificationRepository repository,
            INotifier notifier,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<SendNotificationToRoleHandler> logger)
        {
            _notifier = notifier;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(SendNotificationToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usersInRole = await _userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == request.Request.Role));

                if (!usersInRole.Any())
                    return Result<string>.Failure($"No users found in role {request.Request.Role.ToString()}.");

                await _unitOfWork.BeginTransactionAsync();
                foreach (var user in usersInRole)
                {
                    var notification = new Notification(user.Id, request.Request.Title, request.Request.Message,
               request.Request.Type);

                    await _repository.AddAsync(notification);
                    await _notifier.SendToRoleAsync(request.Request.Role,new NotificationDto 
                    {
                        Id = notification.Id,
                        Message = notification.Message,
                        Timestamp = notification.DateCreated.DateTime,
                        Title = notification.Title,
                        Type = notification.Type
                    });
                }

                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Notification sent to Role {Role}", request.Request.Role.ToString());

                return Result<string>.Success("Notifications sent successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to send notifications to Role {Role}", request.Request.Role.ToString());
                return Result<string>.Failure("Failed to send notifications.");
            }
        }
    }
}

