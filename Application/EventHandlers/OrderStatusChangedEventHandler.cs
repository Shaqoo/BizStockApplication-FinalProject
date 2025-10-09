using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;

namespace Application.EventHandlers
{
    
    public class OrderStatusChangedEventHandler(
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        INotifier notifier,
        IUnitOfWork unitOfWork)
        : INotificationHandler<OrderStatusChangedEvent>
    {
        public async Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(notification.CustomerEmail ?? "");
            if (user is null)
                return;

             
            var (title, message) = GetNotificationText(notification);

            
            var link = $"/sales/orders/{notification.OrderId}";

             
            var appNotification = new Notification(
                user.Id,
                title,
                message,
                "info",
                link
            );

            await notificationRepository.AddAsync(appNotification);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            
            await notifier.SendNotificationAsync(user.Id, new NotificationDto
            {
                Id = appNotification.Id,
                Title = title,
                Message = message,
                Type = "info",
                IsRead = appNotification.IsRead
            });
        }

        private static (string Title, string Message) GetNotificationText(OrderStatusChangedEvent evt)
        {
            var baseMessage = evt.Message ?? "Your order status has been updated.";

            var trackingUrl = $"https://bizstock.com/orders/track/{evt.OrderId}";

            return evt.NewStatus.ToLower() switch
            {
                "pending" or "pending pickup" => (
                    "Order Placed Successfully 🎉",
                    "We’ve received your order and it’s being prepared for pickup."
                ),
                "picked up" => (
                    "Your Package Has Been Picked Up 📦",
                    "Our dispatch rider has picked up your package and it’s heading to our sorting facility.\n" +
                     $"Track your order here: {trackingUrl}"
                ),
                "dispatched" => (
                    "Your Order Has Been Dispatched 🚚",
                    "Your package is now en route to the delivery address.\n" +
                    $"Track your order here: {trackingUrl}"
                ),
                "in transit" => (
                    "Your Package Is In Transit 🚀",
                    "Your order is on the way and will reach your location soon.\n"+
                    $"Track your order here: {trackingUrl}"
                ),
                "out for delivery" => (
                    "Out For Delivery 🚴‍♂️",
                    "Your package is out for delivery today. Please ensure someone is available to receive it. \n"
                    + $"Track your order here: {trackingUrl}"
                ),
                "delivered" => (
                    "Order Delivered Successfully ✅",
                    "Your package has been successfully delivered. Thank you for shopping with us!"
                ),
                "failed" or "cancelled" => (
                    "Delivery Attempt Failed ⚠️",
                    "Unfortunately, your package could not be delivered. Please contact support for assistance."
                ),
                _ => (
                    $"Order Update: {evt.NewStatus}",
                    baseMessage
                )
            };
        }
    }
}
