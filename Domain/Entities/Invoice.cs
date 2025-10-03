using Domain.Auditable;
using Domain.Enums;

namespace Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; private set; } = default!;
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;
        public Guid? SalesOrderId { get; private set; }
        public SalesOrder? SalesOrder { get; private set; }
        public ICollection<Payment> Payments { get; private set; } = new HashSet<Payment>();
        public DateTime? DueDate { get; private set; }
        public decimal SubTotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Tax { get; private set; }
        public decimal TotalAmount => SubTotal - Discount + Tax;

        public InvoiceStatus Status { get; private set; } = InvoiceStatus.Unpaid;

        public ICollection<InvoiceItem> Items { get; private set; } = new HashSet<InvoiceItem>();

        private Invoice() { }

        public Invoice(string invoiceNumber, Guid customerId, decimal discount, decimal tax)
        {
            InvoiceNumber = invoiceNumber;
            CustomerId = customerId;
            Discount = discount;
            Tax = tax;
            Status = InvoiceStatus.Unpaid;
        }

        public void AddItem(Guid productId, string description, int quantity, decimal unitPrice)
        {
            var item = new InvoiceItem(productId, description, quantity, unitPrice, this.Id);
            Items.Add(item);
            RecalculateSubTotal();
        }

        public decimal AmountPaid => Payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount);

        public decimal BalanceDue => TotalAmount - AmountPaid;
        public void RecalculateSubTotal()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
        }

        public void MarkAsPaid()
        {
            Status = InvoiceStatus.Paid;
        }

        public void MarkAsCancelled()
        {
            Status = InvoiceStatus.Cancelled;
        }
    }

}
