using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class PaymentRefundedEvent : INotification
    {
        public Guid PaymentId { get; init; }
        public Guid InvoiceId { get; init; }
        public Guid PayerId { get; init; }
        public decimal Amount { get; init; }
        public string? Reason { get; init; }
        public DateTime RefundedAt { get; init; }

        public PaymentRefundedEvent(Guid paymentId, Guid invoiceId, Guid payerId, decimal amount, string? reason, DateTime refundedAt)
        {
            PaymentId = paymentId;
            InvoiceId = invoiceId;
            PayerId = payerId;
            Amount = amount;
            Reason = reason;
            RefundedAt = refundedAt;
        }
    }

}
