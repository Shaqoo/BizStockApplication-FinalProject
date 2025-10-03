using Domain.Enums;

namespace Application.Dto
{
    public record PaymentDto
    {
        public Guid Id { get; set; }
        public string PaymentReference { get; set; } = default!;
        public Guid? InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } = default!;
        public PaymentStatus Status { get; set; } = default!;
        public string? Note { get; set; }
        public Guid PayerId { get; set; }
        public string PayerName { get; set; } = default!;
        public PaymentPurpose Purpose { get; set; } = default!;
        public Guid? WalletTransactionId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class PaystackVerifyResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; } = default!;
        public PaystackVerifyData Data { get; set; } = default!;
    }

    public class PaystackVerifyData
    {
        public string Status { get; set; } = default!;
        public string Reference { get; set; } = default!;
        public int Amount { get; set; }  
        public string Gateway_Response { get; set; } = default!;
        public DateTime? Paid_At { get; set; }
    }


}
