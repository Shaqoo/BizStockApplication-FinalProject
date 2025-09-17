namespace Application.Dto.RequestModels
{
    public record UpdatePurchaseOrderDto(
    Guid PurchaseOrderId,
    string? Notes,
    decimal Discount,
    decimal Tax
);


    public record AddPurchaseOrderItemDto(
    Guid PurchaseOrderId,
    Guid ProductId,
    string ProductName,
    int QuantityOrdered,
    decimal UnitPrice
);


    public record UpdatePurchaseOrderItemDto(
    Guid PurchaseOrderId,
    Guid PurchaseOrderItemId,
    int QuantityOrdered,
    decimal UnitPrice
);

    public record RemovePurchaseOrderItemDto(
    Guid PurchaseOrderId,
    Guid PurchaseOrderItemId
);

}
