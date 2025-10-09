namespace Application.Dto.RequestModels
{
    public class ReviewCreatedProductDto
    {
        public required Guid ProductId { get; set; }
        public required bool Approved { get; set; }
    }

}
