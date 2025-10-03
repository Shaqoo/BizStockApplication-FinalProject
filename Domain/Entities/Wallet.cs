using Domain.Auditable;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;
        public decimal Balance { get; private set; } = 0m;
        public bool IsActive { get; private set; } = true;

        private readonly List<WalletTransaction> _transactions = new();
        public IReadOnlyCollection<WalletTransaction> Transactions => _transactions.AsReadOnly();
        public string PinHash { get; private set; } = default!;

        private Wallet() { }

        public Wallet(Guid userId)
        {
            CustomerId = userId;
            Balance = 0;
            IsActive = true;
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException("Credit amount must be positive.");

            Balance += amount;
        }

        public void Debit(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException("Debit amount must be positive.");

            if (Balance < amount)
                throw new InsufficientBalanceException(Balance,amount);

            Balance -= amount;
        }

        public void Deactivate() => IsActive = false;
        public void Reactivate() => IsActive = true;
        public void SetPin(string rawPin)
        {
            PinHash = rawPin;
        }

        public bool VerifyPin(string rawPin)
        {
            return PinHash == rawPin;
        }

    }

}
