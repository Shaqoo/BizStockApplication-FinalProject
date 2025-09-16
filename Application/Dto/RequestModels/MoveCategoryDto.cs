namespace Application.Dto.RequestModels
{

    public record MoveCategoryDto(
        Guid? NewParentCategoryId
    );
}
