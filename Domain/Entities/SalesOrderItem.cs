using Domain.Auditable;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalesOrderItem : BaseEntity
    {
        public Guid SalesOrderId { get; private set; }
        public SalesOrder SalesOrder { get; private set; } = default!;

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public string ProductName { get; private set; } = default!;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        private SalesOrderItem() { }

        public SalesOrderItem(Guid productId, string productName, int quantity, decimal unitPrice, Guid salesOrderId)
        {
            if (quantity <= 0) throw new DomainException("Quantity must be positive.");
            if (unitPrice < 0) throw new DomainException("Price cannot be negative.");

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            SalesOrderId = salesOrderId;
        }
    }

}
