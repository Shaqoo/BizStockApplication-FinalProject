using Domain.Auditable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InvoiceItem : BaseEntity
    {
        public Guid InvoiceId { get; private set; }
        public Invoice Invoice { get; private set; } = default!;
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        private InvoiceItem() { }

        public InvoiceItem(Guid productId, string description, int quantity, decimal unitPrice, Guid invoiceId)
        {
            ProductId = productId;
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
            InvoiceId = invoiceId;
        }
    }

}
