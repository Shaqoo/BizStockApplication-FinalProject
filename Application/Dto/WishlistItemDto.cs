namespace Application.Dto
{
    public record WishlistItemDto
    {
        public Guid Id { get; set; }
        public Guid WishlistId { get; set; }
        public Guid ProductId { get; set; }
        public string BrandName { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public decimal ProductPrice { get; set; }
        public string ProductImageUrl { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
    }

}
