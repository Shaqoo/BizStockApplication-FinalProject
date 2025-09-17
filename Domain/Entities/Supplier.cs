using Domain.Auditable;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public Guid UserId { get; private set; }

        public User User { get; private set; } = default!;
        public string CompanyName { get; private set; } = default!;
        public string? Address { get; private set; }

        public string ContactPerson { get; private set; } = default!;

        public PhoneNumber PhoneNumber { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string? TaxId { get; private set; }

        public ICollection<Product> ProductsSupplied { get; private set; } = new HashSet<Product>();
        public ICollection<PurchaseOrder> PurchaseOrders { get; private set; } = new HashSet<PurchaseOrder>();

        public ICollection<Review> Reviews { get; private set; } = new HashSet<Review>();

        private Supplier() { }

        public Supplier(Guid userId, string companyName, PhoneNumber phone,Email email, string contactPerson , string? address = null, string? taxId = null)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                throw new DomainException("Company name is required.");

            Email = email;
            UserId = userId;
            CompanyName = companyName;
            Address = address;
            ContactPerson = contactPerson;
            PhoneNumber = phone;
            TaxId = taxId;
        }

        public void UpdateDetails(string? address, string contactPerson, PhoneNumber phone, string? taxId)
        {
            Address = address;
            ContactPerson = contactPerson;
            PhoneNumber = phone;
            TaxId = taxId;
            Modified();
        }
    }
}
