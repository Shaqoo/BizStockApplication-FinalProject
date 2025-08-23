namespace Application.Dto
{
    public class AddRecentlyViewedProductRequest
    {
        public Guid? UserId { get; set; }  
        public string? SessionId { get; set; } 
        public Guid ProductId { get; set; }
    }

    public record RecentlyViewedProductDto
    {
        public Guid ProductId { get; set; }
        public DateTimeOffset ViewedAt { get; set; }
    }

    public record RecentlyViewedProductsDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? SessionId { get; set; }
        public IReadOnlyCollection<RecentlyViewedProductDto> Items { get; set; } = [];
    }

}
