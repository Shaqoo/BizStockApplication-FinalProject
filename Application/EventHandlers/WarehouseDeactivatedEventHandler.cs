using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using Mailjet.Client.Resources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class WarehouseDeactivatedEventHandler(IUserRepository userRepository,
        INotifier notifier,
        INotificationRepository notificationRepository,
        ILogger<WarehouseCreatedEvent> logger,
        IUnitOfWork unitOfWork) : INotificationHandler<WarehouseDeactivatedEvent>
    {
        public async Task Handle(WarehouseDeactivatedEvent notification, CancellationToken cancellationToken)
        {
            string title = "Warehouse Deactivated";
            string message = $"The warehouse \"{notification.Name}\" located at \"{notification.Location}\" has been deactivated.";

            var users = await userRepository.FindAsync(a =>
                a.UserRoles.Any(a => a.Role == Role.InventoryManager) ||
                a.UserRoles.Any(a => a.Role == Role.Admin) ||
                a.UserRoles.Any(a => a.Role == Role.Manager));

            await unitOfWork.BeginTransactionAsync();

            foreach (var user in users)
            {
                await notificationRepository.AddAsync(new Notification(user.Id, title, message, "warning"));

                logger.LogInformation("Deactivation notification sent to {Email} for warehouse {WarehouseName}", user.Email.Value, notification.Name);
            }

            await unitOfWork.CommitTransactionAsync();

            foreach (var user in users)
            {
                await notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Title = title,
                    Message = message,
                    Type = "warning",
                    Timestamp = DateTime.UtcNow
                });
            }

        }

    }
}
