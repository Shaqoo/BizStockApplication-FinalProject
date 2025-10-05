using Domain.Enums;

namespace Application.Dto
{
    public record InvoiceItemDto
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }

    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = default!;

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = default!; 

        public Guid SalesOrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount => SubTotal - Discount + Tax;

        public InvoiceStatus Status { get; set; } = default!;

        public List<InvoiceItemDto> Items { get; set; } = new();
    }


}
