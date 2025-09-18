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

    public record PurchaseOrderCancelledEvent(
    Guid PurchaseOrderId,
    string OrderNumber,
    Guid SupplierId,
    string Reason
) : INotification;

    public record PurchaseOrderConfirmedEvent(
       Guid PurchaseOrderId,
       string OrderNumber,
       string? Comment,
       Guid SupplierId
   ) : INotification;

    public record PurchaseOrderRejectedEvent(
        Guid PurchaseOrderId,
        string OrderNumber,
        Guid SupplierId,
        string? Reason
    ) : INotification;

    public record PurchaseOrderItemsReceivedEvent(
    Guid PurchaseOrderId,
    string OrderNumber,
    Guid SupplierId,
    List<ReceivedItemEventDto> Items
) : INotification;

    public record ReceivedItemEventDto(
        Guid PurchaseOrderItemId,
        int QuantityReceived
    );


}
