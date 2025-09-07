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
    public class ChatThreadAssignedEventHandler(
    INotifier notifier,
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork)
    : INotificationHandler<ChatThreadAssignedEvent>
    {
        public async Task Handle(ChatThreadAssignedEvent notification, CancellationToken cancellationToken)
        {
            var dto = new NotificationDto
            {
                Title = "Chat Thread Assigned",
                Message = $"A chat thread has been assigned to you.",
                Type = "info",
                ThreadId = notification.ThreadId
            };

            var not = new Notification(notification.AgentId, dto.Title, dto.Message);
            dto.Id = not.Id;



            await notifier.SendNotificationAsync(notification.AgentId, dto);

            await unitOfWork.BeginTransactionAsync();
            await notificationRepository.AddAsync(not);
            await unitOfWork.CommitTransactionAsync();
        }
    }

}
