using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class ChatThreadClosedEventHandler(
    INotifier notifier,
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork)
    : INotificationHandler<ChatThreadClosedEvent>
    {
        public async Task Handle(ChatThreadClosedEvent notification, CancellationToken cancellationToken)
        {
            var dto = new NotificationDto
            {
                Title = "Chat Thread Closed",
                Message = $"The chat thread has been closed.",
                Type = "info",
                ThreadId = notification.ThreadId
            };
            
            await unitOfWork.BeginTransactionAsync();
            if (notification.AgentId != Guid.Empty)
            {
                await notifier.SendNotificationAsync(notification.AgentId, dto);
                await notificationRepository.AddAsync(new Notification(notification.AgentId, dto.Title, dto.Message));
            }

            if (notification.CustomerId != Guid.Empty)
            {
                await notificationRepository.AddAsync(new Notification(notification.CustomerId, dto.Title, dto.Message));
            }

            await unitOfWork.CommitTransactionAsync();

            await notifier.SendNotificationAsync(notification.CustomerId, dto);

        }
    }

}
