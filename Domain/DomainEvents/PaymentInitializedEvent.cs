using MediatR;

namespace Domain.DomainEvents
{
    public class PaymentInitializedEvent : INotification
    {
        public Guid PaymentId { get; }
        public Guid CustomerId { get; }
        public decimal Amount { get; }
        public string Reference { get; }

        public PaymentInitializedEvent(Guid paymentId, Guid customerId, decimal amount, string reference)
        {
            PaymentId = paymentId;
            CustomerId = customerId;
            Amount = amount;
            Reference = reference;
        }
    }

}
