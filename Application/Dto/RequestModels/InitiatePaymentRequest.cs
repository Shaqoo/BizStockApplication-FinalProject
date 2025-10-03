using Domain.Enums;

namespace Application.Dto.RequestModels
{
    public class InitiatePaymentRequest
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } = PaymentMethod.Online;
        public Guid? WalletTransactionId { get; set; }
        public Guid? InvoiceId { get; set; }
        public PaymentPurpose PaymentPurpose { get; set; }
        public string? Note { get; set; }
        public int? Pin { get; set; }
    }

}
