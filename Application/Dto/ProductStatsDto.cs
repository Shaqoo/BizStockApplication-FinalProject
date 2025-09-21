namespace Application.Dto
{
    public record ProductStatsDto
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }

        public int OutOfStockCount { get; set; }

        public int LowStockCount { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int TotalProducts { get; set; }
    }

    public class TopSellingProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalSold { get; set; }     
    }
}
