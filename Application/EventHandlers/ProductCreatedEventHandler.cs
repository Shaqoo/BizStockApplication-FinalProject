using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class ProductCreatedEventHandler(INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        INotifier notifier) : INotificationHandler<ProductCreatedEvent>
    {
        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            const string title = "New Product Added";
            const string type = "info";
            var message = $"New product created: {notification.Name} (SKU: {notification.SKU})";

            var users = await userRepository.FindAsync(u =>
                u.UserRoles.Any(a => a.Role == Role.Admin) ||
                u.UserRoles.Any(a => a.Role == Role.Manager) ||
                u.UserRoles.Any(a => a.Role == Role.InventoryManager)
            );

            if (users == null || !users.Any()) return;

            var notificationDto = new NotificationDto
            {
                Title = title,
                Message = message,
                Type = type,
                Timestamp = DateTime.UtcNow
            };

            await unitOfWork.BeginTransactionAsync();

            foreach (var user in users)
            {
                var dbNotification = new Notification(user.Id, title, message, type);
                await notificationRepository.AddAsync(dbNotification);

                notificationDto.Id = dbNotification.Id;
                await notifier.SendNotificationAsync(user.Id, notificationDto);
            }

            await unitOfWork.CommitTransactionAsync();
        }

    }
}
