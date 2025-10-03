using Domain.Auditable;
using Domain.Enums;

namespace Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public Wallet Wallet { get; private set; } = default!;
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public string Reference { get; private set; } = default!;
        public string? Description { get; private set; }
        public Guid PaymentId { get; private set; }
        public Payment Payment { get; private set; } = default!;
        private WalletTransaction() { }

        public WalletTransaction(Guid walletId, decimal amount, TransactionType type, string reference, Guid paymentId , string? description)
        {
            WalletId = walletId;
            Amount = amount;
            Type = type;
            Reference = reference;
            Description = description;
            PaymentId = paymentId;
        }
    }

}
