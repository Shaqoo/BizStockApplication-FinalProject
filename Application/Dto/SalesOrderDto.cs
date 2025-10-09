using Domain.Enums;

namespace Application.Dto
{
    public record SalesOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = default!;
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = default!;
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public OrderStatus Status { get; set; } = default!;    
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DeliveryStatus OverallDeliveryStatus { get; set; } = default!;
        public Guid? DeliveryAssignmentId { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public List<SalesOrderItemDto> Items { get; set; } = new();
    }

    public record SalesOrderItemDto
    {
        public Guid Id { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string ProductImg { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? UniqueId { get; set; }
        public string? FezOrderNo { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; } = default!;
    }

}
