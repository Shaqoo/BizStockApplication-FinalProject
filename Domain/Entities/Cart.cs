using Domain.Exceptions;

namespace Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public Guid? UserId { get; private set; }  
        public string SessionId { get; private set; }  = default!;
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

        public void AddOrUpdateItem(Guid productId, int quantity)
        {
            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
                existing.IncreaseQuantity(quantity);
            else
                _items.Add(new CartItem(this.Id,productId, quantity));
        }

        public void MarkAsLinked()
        {
            IsLinked = true;
        }
        public void LinkToUser(Guid userId)
        {
            if (IsLinked)
                throw new DomainException("Cart is already linked.");

            UserId = userId;
            IsLinked = true;
        }

         
        public CartItem AddItem(Guid productId, int quantity)
        {
            var existing = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                existing.IncreaseQuantity(quantity);
                return existing;
            }
            else
            {
                _items.Add(new CartItem(Id, productId, quantity));
                return _items.Last();
            }
        }

         
        public bool RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
                return true;
            }
            else
                return false;
        }

         
        public bool UpdateItemQuantity(Guid productId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.SetQuantity(quantity);
                return true;
            }
            else
                return false;
        }

    }
}
