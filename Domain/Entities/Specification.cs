using Domain.Exceptions;

namespace Domain.Entities
{
    public class Specification
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; } = default!;

        private readonly List<ProductSpecification> _productSpecifications = new();
        public IReadOnlyCollection<ProductSpecification> ProductSpecifications => _productSpecifications.AsReadOnly();

        private Specification() { }

        public Specification(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Specification name cannot be empty.");

            Name = name.Trim();
            Description = description;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Specification name cannot be empty.");

            Name = newName.Trim();
        }

        public void UpdateDescription(string? newDescription)
        {
            Description = newDescription;
        }

        public ProductSpecification AddProductSpecification(Guid productId, string value)
        {
            var productSpec = new ProductSpecification(productId, Id, value);
            _productSpecifications.Add(productSpec);
            return productSpec;
        }
    }

}
