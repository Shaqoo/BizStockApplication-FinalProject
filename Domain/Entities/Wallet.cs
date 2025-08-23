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
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        public decimal Balance { get; private set; } = 0m;
        public bool IsActive { get; private set; } = true;

        private readonly List<WalletTransaction> _transactions = new();
        public IReadOnlyCollection<WalletTransaction> Transactions => _transactions.AsReadOnly();
        public string PinHash { get; private set; } = default!;

        private Wallet() { }

        public Wallet(Guid userId)
        {
            UserId = userId;
            Balance = 0;
            IsActive = true;
        }

        public void Credit(decimal amount, string reference, string description)
        {
            if (amount <= 0)
                throw new DomainException("Credit amount must be positive.");

            Balance += amount;

            _transactions.Add(new WalletTransaction(this.Id, amount, TransactionType.Credit, reference, description));
        }

        public void Debit(decimal amount, string reference, string description)
        {
            if (amount <= 0)
                throw new DomainException("Debit amount must be positive.");

            if (Balance < amount)
                throw new InsufficientBalanceException(Balance,amount);

            Balance -= amount;

            _transactions.Add(new WalletTransaction(this.Id, amount, TransactionType.Debit, reference, description));
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
        public void SetPin(string rawPin)
        {
            if (string.IsNullOrWhiteSpace(rawPin) || rawPin.Length != 4)
                throw new DomainException("PIN must be 4 digits.");

            PinHash = rawPin;
        }

        public bool VerifyPin(string rawPin)
        {
            return PinHash == rawPin;
        }

    }

}
