namespace Application.Dto.RequestModels
{
    public record AddProductQuantityDto
    {
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
    }


}
