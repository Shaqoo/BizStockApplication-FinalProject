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

}
