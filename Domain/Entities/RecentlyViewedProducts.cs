namespace Domain.Entities
{
    public class RecentlyViewedProducts
    {
        public Guid Id { get; private set; }
        public Guid? UserId { get; private set; } 
        public string? SessionId { get; private set; } 
        public bool IsLinked { get; private set; }

        private readonly List<RecentlyViewedProduct> _items = new();
        public IReadOnlyCollection<RecentlyViewedProduct> Items => _items.AsReadOnly();
        public DateTimeOffset DateAdded { get; private set; } = DateTimeOffset.UtcNow;

        private const int MaxItems = 10;

        public RecentlyViewedProducts(string sessionId)
        {
            Id = Guid.NewGuid();
            SessionId = sessionId;
            IsLinked = false;
        }

        public RecentlyViewedProducts(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            IsLinked = true;
        }

        public void LinkToUser(Guid userId)
        {
            if (IsLinked)
                throw new InvalidOperationException("Already linked.");
            UserId = userId;
            IsLinked = true;
        }

        public void AddProduct(Guid productId)
        {
            var existing = _items.FirstOrDefault(x => x.ProductId == productId);
            if (existing != null)
                _items.Remove(existing);

            _items.Insert(0, new RecentlyViewedProduct(Id, productId));
            if (_items.Count > MaxItems)
                _items.RemoveAt(_items.Count - 1);
        }

        public bool RemoveItem(Guid productId)
        {
            var existing = _items.FirstOrDefault(x => x.ProductId == productId);
            if (existing != null)
            {
                _items.Remove(existing);
                return true;
            }
            return false;
        }
        public void ClearItems()
        {
            _items.Clear();
        }
    }
}
