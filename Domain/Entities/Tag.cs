using Domain.Auditable;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public ICollection<ProductTag> ProductTags { get; private set; } = new HashSet<ProductTag>();

        private Tag() { }
        public Tag(string name)
        {
            Name = name ?? throw new DomainException(nameof(name));
        }

        public void UpdateName(string newName)
        {
            Name = newName ?? throw new DomainException(nameof(newName));
        }
        public void AddTag(ProductTag tag)
        {
            if (tag == null) throw new DomainException("Tag cannot be null.");
            if (ProductTags.Any(t => t.Id == tag.Id)) return;
            ProductTags.Add(tag);
        }
    }
}
