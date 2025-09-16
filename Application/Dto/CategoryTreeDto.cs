namespace Application.Dto
{
    public class CategoryTreeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int Depth { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public List<CategoryTreeDto> SubCategories { get; set; } = new();
    }

}
