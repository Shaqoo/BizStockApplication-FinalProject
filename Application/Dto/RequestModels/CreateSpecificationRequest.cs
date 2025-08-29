namespace Application.Dto.RequestModels
{
    public record CreateSpecificationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
