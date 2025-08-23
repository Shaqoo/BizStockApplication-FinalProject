namespace Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; }
        public Guid CartId { get; private set; }  
        public Product Product { get; private set; } = default!;
        public Cart Cart { get; private set; }  = default!; 
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        private CartItem() { } 
        public CartItem(Guid cartId, Guid productId , int quantity)
        {
            Id = Guid.NewGuid();
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int qty) => Quantity += qty;
        public void SetQuantity(int qty) => Quantity = qty;
    }


}
