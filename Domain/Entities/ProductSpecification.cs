using Domain.Exceptions;

namespace Domain.Entities
{
    public class ProductSpecification
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;

        public Guid SpecificationId { get; private set; }
        public Specification Specification { get; private set; } = default!;

        public string Value { get; private set; } = default!;

        private ProductSpecification() { }

        public ProductSpecification(Guid productId, Guid specificationId, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Specification value cannot be empty.");

            ProductId = productId;
            SpecificationId = specificationId;
            Value = value.Trim();
        }

        public void UpdateValue(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
                throw new DomainException("Specification value cannot be empty.");

            Value = newValue.Trim();
        }
    }

}
