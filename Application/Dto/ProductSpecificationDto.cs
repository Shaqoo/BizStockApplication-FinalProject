namespace Application.Dto
{
    public record ProductSpecificationDto
    {
        public Guid Id { get; init; }
        public Guid ProductId { get; init; }
        public Guid SpecificationId { get; init; }
        public string SpecificationName { get; init; } = string.Empty; 
        public string Value { get; init; } = string.Empty;
    }

    public record SpecificationDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }

    public record ProductSpecificationListDto
    {
        public Guid ProductId { get; init; }
        public List<ProductSpecificationDto> Specifications { get; init; } = new();
    }


}
