namespace Application.Dto.RequestModels
{
    public record AddProductSpecificationRequest
    {
        public Guid ProductId { get; set; }
        public Guid SpecificationId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
