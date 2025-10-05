using Domain.Auditable;
using Domain.Enums;

namespace Domain.Entities
{
    public class SalesOrder : BaseEntity
    {
        public string OrderNumber { get; private set; } = default!; 
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;
        public Invoice Invoice { get; private set; } = default!; 
        public Guid InvoiceId { get; private set; }
        public DateTime? ExpectedDeliveryDate { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal SubTotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Tax { get; private set; }
        public decimal Total => SubTotal - Discount + Tax;
        public ICollection<SalesOrderItem> Items { get; private set; } = new HashSet<SalesOrderItem>();
        public DeliveryAssignment DeliveryAssignment { get; private set; } = default!; 
        public Guid? DeliveryAssignmentId { get; private set; }
        public string? Note { get; private set; }

        public DeliveryStatus OverallDeliveryStatus =>
           Items.All(i => i.DeliveryStatus == DeliveryStatus.Delivered)
               ? DeliveryStatus.Delivered
               : Items.Any(i => i.DeliveryStatus == DeliveryStatus.InTransit)
                   ? DeliveryStatus.InTransit
                   : DeliveryStatus.Pending;

        private SalesOrder() { }

        public SalesOrder(string orderNumber, Guid customerId, decimal discount, decimal tax, DateTime? expectedDeliveryDate = null, string? note = null, Guid invoiceId = default, Guid? deliveryAssignmentId = null)
        {
            OrderNumber = orderNumber;
            CustomerId = customerId;
            Discount = discount;
            Tax = tax;
            ExpectedDeliveryDate = expectedDeliveryDate;
            Note = note;
            InvoiceId = invoiceId;
            DeliveryAssignmentId = deliveryAssignmentId;
        }

        public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            var item = new SalesOrderItem(productId, productName, quantity, unitPrice, this.Id);
            Items.Add(item);
            RecalculateTotals();
        }

        public void RecalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
        }

        public void AddDeliveryAssignment(Guid deliveryAssignmentId)
        {
            DeliveryAssignmentId = deliveryAssignmentId;
            Modified();
        }

        public void AddInvoice(Guid invoiceId)
        {
            InvoiceId = invoiceId;
            Modified();
        }

        public void MarkAsConfirmed() => Status = OrderStatus.Confirmed;
        public void MarkAsShipped() => Status = OrderStatus.Shipped;
        public void MarkAsProcessing() => Status = OrderStatus.Processing;

        public void MarkAsCancelled() => Status = OrderStatus.Cancelled;
        public void MarkAsDelivered() => Status = OrderStatus.DeliveredPendingConfirmation;
        public void MarkAsCompleted() => Status = OrderStatus.Completed;
    }

}
