namespace Application.Dto.RequestModels
{
    public record CreateCategoryDto(
    string Name,
    string? Description,
    Guid? ParentCategoryId
);
}
