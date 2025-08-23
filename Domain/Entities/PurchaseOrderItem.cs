using Domain.Auditable;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PurchaseOrderItem : BaseEntity
    {
        public Guid PurchaseOrderId { get; private set; }
        public PurchaseOrder PurchaseOrder { get; private set; } = default!;

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;

        public string ProductName { get; private set; } = default!;
        public int QuantityOrdered { get; private set; }
        public int QuantityReceived { get; private set; }
        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice => QuantityOrdered * UnitPrice;
        public bool IsFullyReceived => QuantityReceived >= QuantityOrdered;

        private PurchaseOrderItem() { }

        public PurchaseOrderItem(Guid productId, string productName, int quantity, decimal unitPrice, Guid purchaseOrderId)
        {
            if (quantity <= 0) throw new DomainException("Quantity must be positive.");
            if (unitPrice < 0) throw new DomainException("Price cannot be negative.");

            ProductId = productId;
            ProductName = productName;
            QuantityOrdered = quantity;
            UnitPrice = unitPrice;
            PurchaseOrderId = purchaseOrderId;
        }

        public void Receive(int quantity)
        {
            if (quantity <= 0) throw new DomainException("Received quantity must be positive.");
            if (QuantityReceived + quantity > QuantityOrdered)
                throw new DomainException("Cannot receive more than ordered quantity.");

            QuantityReceived += quantity;
        }
    }

}
