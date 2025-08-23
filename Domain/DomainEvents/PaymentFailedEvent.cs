using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class PaymentFailedEvent : INotification
    {
        public Guid PaymentId { get; init; }
        public Guid InvoiceId { get; init; }
        public Guid PayerId { get; init; }
        public string? FailureReason { get; init; }
        public DateTime FailedAt { get; init; }

        public PaymentFailedEvent(Guid paymentId, Guid invoiceId, Guid payerId, string? failureReason, DateTime failedAt)
        {
            PaymentId = paymentId;
            InvoiceId = invoiceId;
            PayerId = payerId;
            FailureReason = failureReason;
            FailedAt = failedAt;
        }
    }

}
