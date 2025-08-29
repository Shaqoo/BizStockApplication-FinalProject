namespace Application.Dto.RequestModels
{
    public record UpdateSpecificationRequest
    {
        public Guid SpecificationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
