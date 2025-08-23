using Domain.Auditable;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string PaymentReference { get; private set; } = default!; 
        public Guid InvoiceId { get; private set; }
        public Invoice Invoice { get; private set; } = default!;
        public decimal Amount { get; private set; }
        public PaymentMethod Method { get; private set; } = PaymentMethod.Wallet;
        public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
        public string? Note { get; private set; }

        public Guid PayerId { get; private set; } 
        public User Payer { get; private set; } = default!;

        private Payment() { }

        public Payment(
            string paymentReference,
            Guid invoiceId,
            decimal amount,
            PaymentMethod method,
            Guid payerId,
            string? note = null
        )
        {
            if (amount <= 0)
                throw new DomainException("Payment amount must be greater than zero.");

            PaymentReference = paymentReference;
            InvoiceId = invoiceId;
            Amount = amount;
            Method = method;
            PayerId = payerId;
            Note = note;
        }

        public void MarkAsCompleted()
        {
            Status = PaymentStatus.Completed;
        }

        public void MarkAsFailed(string? note = null)
        {
            Status = PaymentStatus.Failed;
            Note = note ?? Note;
        }

        public void MarkAsRefunded(string? note = null)
        {
            Status = PaymentStatus.Refunded;
            Note = note ?? Note;
        }
    }

}
