using Domain.Auditable;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Email Email { get; private set; } = default!;
        public string FullName { get; private set; } = default!;
        public Guid CustomerTypeId { get; private set; }
        public ICollection<ChatThread> ChatThreads { get; private set; } = new List<ChatThread>();
        public ICollection<SalesOrder> SalesOrders { get; private set; } = new List<SalesOrder>();
        public ICollection<Invoice> Invoices { get; private set; } = new List<Invoice>();
        public CustomerType CustomerType { get; private set; } = default!;
        public string? BusinessName { get; private set; }

        public string? Address { get; private set; }

        public string? State { get; private set; }

        public string? TaxId { get; private set; }

        private Customer() { }

        public Customer(Email email, Guid customerTypeId,string fullname, string? businessName = null, string? address = null, string? taxId = null)
        {
            Email = email;
            CustomerTypeId = customerTypeId;
            FullName = fullname;
            BusinessName = businessName;
            Address = address;
            TaxId = taxId;
        }

        public void ChangeCustomerType(Guid newTypeId)
        {
            if (newTypeId == Guid.Empty)
                throw new DomainException("Invalid customer type.");
            CustomerTypeId = newTypeId;
            Modified();
        }
    }

}
