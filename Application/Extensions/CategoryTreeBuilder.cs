using Application.Dto;
using Domain.Entities;

namespace Application.Extensions
{
    public static class CategoryTreeBuilder
    {
        public static List<CategoryTreeDto> BuildHierarchy(List<Category> categories, Guid? parentId = null)
        {
            return categories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => new CategoryTreeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Depth = c.Depth,
                    ParentCategoryId = c.ParentCategoryId,
                    SubCategories = BuildHierarchy(categories, c.Id)
                })
                .ToList();
        }
    }

}
