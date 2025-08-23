namespace Application.Dto.RequestModels
{
    public record UpdateCartItemQuantityRequest
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
