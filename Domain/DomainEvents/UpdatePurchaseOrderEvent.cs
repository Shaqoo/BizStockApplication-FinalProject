using MediatR;

namespace Domain.DomainEvents
{
    public record PurchaseOrderUpdatedEvent(Guid PurchaseOrderId,
    string orderNumber,
    Guid SupplierId,
    string? Notes,
    decimal Discount,
    decimal Tax) : INotification;
    public record PurchaseOrderItemAddedEvent(Guid PurchaseOrderId,
    string orderNumber,
    Guid SupplierId,
    Guid ProductId,
    string ProductName,
    int QuantityOrdered,
    decimal UnitPrice) : INotification;
    public record PurchaseOrderItemUpdatedEvent(Guid PurchaseOrderId,
    string orderNumber,
    Guid SupplierId,
    Guid PurchaseOrderItemId,
    int QuantityOrdered,
    decimal UnitPrice) : INotification;
    public record PurchaseOrderItemRemovedEvent(Guid PurchaseOrderId, Guid ItemId, string orderNumber,
    Guid SupplierId) : INotification;

}
