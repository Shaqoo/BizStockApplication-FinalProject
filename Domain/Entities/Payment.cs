using Domain.Auditable;
using Domain.Enums;

namespace Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string PaymentReference { get; private set; } = default!;
        public Guid? InvoiceId { get; private set; }
        public Invoice? Invoice { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentMethod Method { get; private set; } = PaymentMethod.Wallet;
        public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
        public string? Note { get; private set; }
        public Guid PayerId { get; private set; }
        public Customer Payer { get; private set; } = default!;
        public PaymentPurpose Purpose { get; private set; }
       // public Guid? WalletTransactionId { get; private set; }
        public WalletTransaction? WalletTransaction { get; private set; }

        private Payment() { }

        public Payment(string paymentReference, Guid payerId, decimal amount, PaymentMethod method, PaymentPurpose purpose,Guid? walletTransactionId, Guid? invoiceId = null, string? note = null)
        {
            PaymentReference = paymentReference;
            PayerId = payerId;
            Amount = amount;
            Method = method;
            Purpose = purpose;
            InvoiceId = invoiceId;
           // WalletTransactionId = walletTransactionId;
            Note = note;
        }

        //public void LinkToTransaction(Guid transactionId)
        //{
        //    WalletTransactionId = transactionId;
        //    Modified();
        //}
        public void MarkAsCompleted()
        {
            Status = PaymentStatus.Completed;
            Modified();
        }

        public void MarkAsFailed(string? note = null)
        {
            Status = PaymentStatus.Failed;
            Note = note;
        }

        public void AddInvoice(Guid invoiceId)
        {
            InvoiceId = invoiceId;
            Modified();
        }
    }

}
