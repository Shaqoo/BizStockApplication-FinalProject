namespace Application.Dto
{
    public record CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string ProductImg { get; set; } = default!;
        public decimal SubTotal => UnitPrice * Quantity;
    }
}
