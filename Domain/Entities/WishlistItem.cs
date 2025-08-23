using Domain.Entities.Domain.Entities;

namespace Domain.Entities
{
    public class WishlistItem
    {
        public Guid Id { get; private set; }
        public Wishlist Wishlist { get; private set; } = default!;
        public Guid WishlistId { get; private set; }
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public WishlistItem(Guid wishlistId, Guid productId)
        {
            Id = Guid.NewGuid();
            WishlistId = wishlistId;
            ProductId = productId;
        }
    }
}

