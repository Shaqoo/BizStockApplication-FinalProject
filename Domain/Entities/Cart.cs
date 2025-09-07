using Domain.Exceptions;

namespace Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public Guid? UserId { get; private set; }
        public string? SessionId { get; private set; } = default!;
        public bool IsLinked { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        private Cart() { }

        public Cart(string sessionId)
        {
            Id = Guid.NewGuid();
            SessionId = sessionId;
            IsLinked = false;
        }

        public Cart(Guid userId, string sessionId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            SessionId = sessionId;
            IsLinked = true;
        }

        public Cart(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            IsLinked = true;
        }

        public CartItem AddOne(Guid productId)
        {
            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                existing.IncreaseQuantity(1);
                return existing;
            }

            var newItem = new CartItem(Id, productId, 1);
            _items.Add(newItem);
            return newItem;
        }

        public bool DecreaseOne(Guid productId)
        {
            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing == null) return false;

            if (existing.Quantity > 1)
            {
                existing.SetQuantity(existing.Quantity - 1);
                return true;
            }
            _items.Remove(existing);
            return true;
        }

        public bool RemoveItem(Guid productId)
        {
            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                _items.Remove(existing);
                return true;
            }
            return false;
        }

        public void LinkToUser(Guid userId)
        {
            if (IsLinked)
                throw new DomainException("Cart is already linked.");

            UserId = userId;
            IsLinked = true;
        }

        public void AddOrUpdateItem(Guid productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var newItem = new CartItem(Id, productId, quantity);
                _items.Add(newItem);
            }
        }
        public void ClearItems()
        {
            _items.Clear();
        }

        public void MarkAsLinked() => IsLinked = true;
    }
}
