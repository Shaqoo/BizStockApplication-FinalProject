namespace Domain.Entities
{
    public class RecentlyViewedProduct
    {
        public Guid Id { get; private set; }
        public Guid RecentlyViewedProductsId { get; private set; }
        public RecentlyViewedProducts RecentlyViewedProducts { get; private set; } = default!;
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public DateTimeOffset DateReviewed { get; private set; } = DateTimeOffset.UtcNow;

        public RecentlyViewedProduct(Guid recentlyViewedProductsId, Guid productId)
        {
            Id = Guid.NewGuid();
            RecentlyViewedProductsId = recentlyViewedProductsId;
            ProductId = productId;
        }
    }
}
