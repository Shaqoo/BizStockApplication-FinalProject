namespace Application.Dto.RequestModels
{
    public class UpdateProductSpecificationRequest
    {
        public Guid ProductSpecificationId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
