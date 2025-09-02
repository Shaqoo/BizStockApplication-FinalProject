using Application.Pagination;

namespace Application.Dto
{
  
    public record CartDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public bool IsLinked { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.SubTotal);
        public decimal TotalQuantity { get; set; }
    }

    public record PaginatedCartDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public bool IsLinked { get; set; }
        public PaginatedList<CartItemDto> Items { get; set; } = new();
        public decimal TotalPrice { get; init; }
        public decimal TotalQuantity { get; init; }
    }

}
