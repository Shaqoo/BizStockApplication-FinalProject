using Domain.Auditable;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public Wallet Wallet { get; private set; } = default!;
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public string Reference { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        private WalletTransaction() { }

        public WalletTransaction(Guid walletId, decimal amount, TransactionType type, string reference, string description)
        {
            WalletId = walletId;
            Amount = amount;
            Type = type;
            Reference = reference;
            Description = description;
        }
    }

}
