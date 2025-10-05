using Domain.Enums;

namespace Application.Dto
{
    public class RefundDto
    {
        public Guid Id { get; set; }
        public Guid SalesOrderId { get; set; }
        public string OrderNumber { get; set; } = default!;
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = default!;
        public string TransactionReference { get; set; } = default!;
        public string RefundReference { get; set; } = default!;
        public RefundStatus Status { get; set; } = default!; 
        public string Reason { get; set; } = default!;
        public DateTime RequestedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

}
