namespace Domain.Entities
{
    namespace Domain.Entities
    {
        public class Wishlist
        {
            public Guid Id { get; private set; }
            public Guid UserId { get; private set; }  

            private readonly List<WishlistItem> _items = new();
            public IReadOnlyCollection<WishlistItem> Items => _items.AsReadOnly();

            public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
             
            public Wishlist(Guid userId)
            {
                Id = Guid.NewGuid();
                UserId = userId;
            }

             
            public void AddItem(Guid productId)
            {
                if (_items.Any(i => i.ProductId == productId))
                    return;  

                _items.Add(new WishlistItem(Id, productId));
            }

             
            public void RemoveItem(Guid productId)
            {
                var item = _items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                    _items.Remove(item);
            }
        }

    }

}
