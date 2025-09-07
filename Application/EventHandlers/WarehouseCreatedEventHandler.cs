using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class WarehouseCreatedEventHandler(IUserRepository userRepository,
        INotifier notifier,
        INotificationRepository notificationRepository,
        ILogger<WarehouseCreatedEvent> logger,
        IUnitOfWork unitOfWork) : INotificationHandler<WarehouseCreatedEvent>
    {
        public async Task Handle(WarehouseCreatedEvent notification, CancellationToken cancellationToken)
        {
            string title = "New Warehouse Created";
              string message = $"A new warehouse has been created with the following details:\n" +
                          $"Name: {notification.Name}\n" +
                          $"Location: {notification.Location}";
                 

            var users = await userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == Role.InventoryManager) 
            || a.UserRoles.Any(a => a.Role == Role.Admin)
           || a.UserRoles.Any(a => a.Role == Role.Manager));

            await unitOfWork.BeginTransactionAsync();
            foreach (var user in users)
            {
                var userNotification = new Notification(user.Id, title, message, "info");
                await notificationRepository.AddAsync(userNotification);

                await notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Id = userNotification.Id,
                    Title = title,
                    Message = message,
                    Type = "info",
                    Timestamp = DateTime.UtcNow
                });

                logger.LogInformation("Notification sent to {Email} for new warehouse creation", user.Email.Value);
            }
            await unitOfWork.CommitTransactionAsync();
        }
    }
}
