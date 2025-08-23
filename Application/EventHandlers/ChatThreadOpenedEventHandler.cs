using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MassTransit.Middleware;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class ChatThreadOpenedEventHandler(INotifier notifier,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork) : INotificationHandler<ChatThreadCreatedEvent>
    {
        public async Task Handle(ChatThreadCreatedEvent notification, CancellationToken cancellationToken)
        {
            var dto = new NotificationDto
            {
                Title = "New Chat Thread",
                Message = $"{notification.name} just started a new conversation.",
                Type = "info",
                ThreadId = notification.ThreadId
            };

            await unitOfWork.BeginTransactionAsync();

            var customerServiceAgents = await userRepository.FindAsync(a => a.HasRole(Role.CustomerService));
            foreach (var agent in customerServiceAgents)
            {
                await notificationRepository.AddAsync(new Notification(agent.Id, dto.Title, dto.Message));
            }

            await unitOfWork.CommitTransactionAsync();

            await notifier.BroadcastToCustomerServiceAsync(dto);

        }


    }
}
