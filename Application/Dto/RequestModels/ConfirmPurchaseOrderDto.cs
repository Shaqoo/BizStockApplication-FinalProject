namespace Application.Dto.RequestModels
{
    public record ConfirmPurchaseOrderDto(
    DateTime ExpectedDeliveryDate,
    string? Notes
   );

    public record ReceivePurchaseOrderItemDto(
    Guid PurchaseOrderItemId,
    int QuantityReceived
    );

    public record CancelPurchaseOrderDto(
    string Reason
    );

    public record RejectPurchaseOrderDto(
    string Reason
    );

}
