using Domain.Enums;
using MediatR;

namespace Domain.DomainEvents
{
    public class PaymentStatusChangedEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid CustomerId { get; }
        public PaymentStatus Status { get; }
        public decimal Amount { get; }
        public string Reference { get; }

        public PaymentStatusChangedEvent(Guid paymentId, Guid customerId, PaymentStatus status, decimal amount, string reference)
        {
            PaymentId = paymentId;
            CustomerId = customerId;
            Status = status;
            Amount = amount;
            Reference = reference;
        }
    }

}
