using MediatR;

namespace Domain.DomainEvents;

public class RefundProcessedEvent : INotification
{
    public Guid RefundId { get; }
    public Guid SalesOrderId { get; }
    public string SalesOrderNumber { get; }
    public decimal Amount { get; }
    public string PaymentMethod { get; }
    public string RefundReference { get; }
    public string Status { get; }
    public string Reason { get; }
    public Guid customerId { get; }
    public DateTime ProcessedAt { get; }

    public RefundProcessedEvent(
        Guid refundId,
        Guid salesOrderId,
        string salesOrderNumber,
        decimal amount,
        string paymentMethod,
        string refundReference,
        string status,
        string reason,
        Guid customerId,
        DateTime processedAt)
    {
        RefundId = refundId;
        SalesOrderId = SalesOrderId;
        SalesOrderNumber = salesOrderNumber;
        Reason = reason;
        Amount = amount;
        PaymentMethod = paymentMethod;
        RefundReference = refundReference;
        Status = status;
        ProcessedAt = processedAt;
    }
}
