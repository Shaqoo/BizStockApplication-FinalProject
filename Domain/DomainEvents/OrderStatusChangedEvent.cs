using MediatR;

namespace Domain.DomainEvents
{
    public class OrderStatusChangedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OldStatus { get; }
        public string NewStatus { get; }
        public string? TrackingNumber { get; }
        public DateTime ChangedAt { get; }
        public string? CustomerEmail { get; }
        public string? CustomerName { get; }
        public string? Message { get; }

        public OrderStatusChangedEvent(
            Guid orderId,
            string oldStatus,
            string newStatus,
            string? trackingNumber,
            string? customerEmail,
            string? customerName,
            string? message = null)
        {
            OrderId = orderId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            TrackingNumber = trackingNumber;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            Message = message;
            ChangedAt = DateTime.UtcNow;
        }
    }

}
