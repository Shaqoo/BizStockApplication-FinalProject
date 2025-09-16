using Domain.Enums;

namespace Application.Dto.RequestModels
{
    public record UpdateProductDetailsDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }

}
