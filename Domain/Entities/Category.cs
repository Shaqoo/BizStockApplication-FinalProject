using Domain.Auditable;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public int Depth { get; private set; }
        public Guid? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }
        public ICollection<Category> SubCategories { get; private set; } = new HashSet<Category>();
        public ICollection<Product> Products { get; private set; } = new HashSet<Product>();

        private Category() { }

        public Category(string name, string? description = null, Guid? parentId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name is required.");

            Name = name;
            Description = description;
            ParentCategoryId = parentId;
            Depth = ParentCategory == null ? 0 : ParentCategory.Depth + 1;
        }


        public void Update(string name, string? description)
        {
            Name = name;
            Description = description;
            Modified();
        }

        public void MoveToParent(Guid? newParentId, int newParentDepth)
        {
            ParentCategoryId = newParentId;
            Depth = newParentId == null ? 0 : newParentDepth + 1;
            Modified();
        }

    }

}
