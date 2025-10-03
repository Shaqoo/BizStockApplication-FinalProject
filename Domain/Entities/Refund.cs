using Domain.Auditable;
using Domain.Enums;

namespace Domain.Entities
{
    public class Refund : BaseEntity 
    {
        public Guid SalesOrderId { get; private set; }
        public SalesOrder Order { get; private set; } = default!;

        public decimal Amount { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; } = default!;

        public string TransactionReference { get; private set; } = default!;

        public string RefundReference { get; private set; } = default!;

        public DateTime RequestedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; private set; }

        public RefundStatus Status { get; private set; } = RefundStatus.Pending;
        public string Reason { get; private set; } = default!;

        public Refund() { }

        public Refund(Guid orderId, decimal amount, PaymentMethod method, string transactionRef, string reason)
        {
            SalesOrderId = orderId;
            Amount = amount;
            PaymentMethod = method;
            TransactionReference = transactionRef;
            Reason = reason;
            Status = RefundStatus.Pending;
        }

        public void MarkCompleted(string refundRef)
        {
            Status = RefundStatus.Successful;
            RefundReference = refundRef;
            CompletedAt = DateTime.UtcNow;
            Modified();
        }

        public void MarkFailed(string reason)
        {
            Status = RefundStatus.Failed;
            Reason = reason;
            Modified();
        }
    }
}
