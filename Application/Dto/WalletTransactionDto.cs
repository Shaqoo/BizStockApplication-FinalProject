using Domain.Enums;

namespace Application.Dto
{
    public class WalletTransactionDto
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } 
        public string Reference { get; set; } = default!;
        public string? Description { get; set; }
        public Guid? PaymentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

}
