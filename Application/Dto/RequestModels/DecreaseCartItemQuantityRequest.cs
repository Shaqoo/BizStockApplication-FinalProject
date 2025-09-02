namespace Application.Dto.RequestModels
{
    public class DecreaseCartItemQuantityRequest
    {
        public Guid CartId { get; set; }       
        public Guid ProductId { get; set; }

        public int Quantity = 1;
    }

}
