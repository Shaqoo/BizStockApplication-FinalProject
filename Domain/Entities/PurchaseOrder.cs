using Domain.Auditable;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public string OrderNumber { get; private set; } = default!;  
        public Guid SupplierId { get; private set; }
        public Supplier Supplier { get; private set; } = default!;
        public DateTime? ExpectedDeliveryDate { get; private set; }
        public PurchaseOrderStatus Status { get; private set; } = PurchaseOrderStatus.Draft;
        public decimal SubTotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Total => SubTotal - Discount + Tax;
        public ICollection<PurchaseOrderItem> Items { get; private set; } = new HashSet<PurchaseOrderItem>();
        public string? Notes { get; private set; }

        private PurchaseOrder() { }

        public PurchaseOrder(string orderNumber, Guid supplierId, decimal discount = 0, decimal tax = 0, DateTime? expectedDeliveryDate = null, string? notes = null)
        {
            OrderNumber = orderNumber;
            SupplierId = supplierId;
            Discount = discount;
            Tax = tax;
            ExpectedDeliveryDate = expectedDeliveryDate;
            Notes = notes;
        }

        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            var item = new PurchaseOrderItem(productId, productName, quantity, unitPrice, this.Id);
            Items.Add(item);
            RecalculateSubTotal();
        }

        private void RecalculateSubTotal()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
        }

        public void Confirm()
        {
            if (!Items.Any()) throw new DomainException("Cannot confirm an empty purchase order.");
            Status = PurchaseOrderStatus.Confirmed;
        }

        public void ReceiveItem(Guid productId, int quantityReceived)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) throw new DomainException("Product not found in purchase order.");
            item.Receive(quantityReceived);
            UpdateStatusBasedOnReceipt();
        }

        private void UpdateStatusBasedOnReceipt()
        {
            if (Items.All(i => i.IsFullyReceived))
                Status = PurchaseOrderStatus.Received;
            else if (Items.Any(i => i.QuantityReceived > 0))
                Status = PurchaseOrderStatus.PartiallyReceived;
        }

        public void Cancel()
        {
            if (Status == PurchaseOrderStatus.Received)
                throw new DomainException("Cannot cancel a fully received order.");

            Status = PurchaseOrderStatus.Cancelled;
        }
    }

}
