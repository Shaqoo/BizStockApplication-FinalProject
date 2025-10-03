using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<OrderCreatedEventHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;

        public OrderCreatedEventHandler(
            INotifier notifier,
            IUnitOfWork unitOfWork,
            INotificationRepository notificationRepository,
            ILogger<OrderCreatedEventHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _notifier = notifier;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                
                var title = $"Order {notification.OrderNumber} Created";

               
                var message = $"{notification.Products.Count} item(s), " +
                              $"Subtotal ₦{notification.SubTotal:N2}, " +
                              $"Delivery ₦{notification.DeliveryCost:N2}, " +
                              $"Total ₦{notification.Total:N2}";

               
                var productNames = string.Join(", ", notification.Products.Select(p => p.Name));
                message += $". Products: {productNames}";

                var linkUrl = $"/orders/{notification.OrderId}"; 

                var notif = new Notification(
                    recipientId: notification.UserId, 
                    title: title,
                    message: message,
                    type: "success",
                    linkUrl: linkUrl
                );

                await _notificationRepository.AddAsync(notif);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _notifier.SendNotificationAsync(notification.UserId,new NotificationDto
                {
                    Id = notif.Id,
                    Title = notif.Title,
                    Message = notif.Message,
                    Type = notif.Type,
                    IsRead = notif.IsRead
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create in-app notification for Order {OrderNumber}",
                    notification.OrderNumber);
            }
        }
    }

}
