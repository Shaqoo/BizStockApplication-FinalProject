namespace Application.Dto.RequestModels
{
    public record CreatePurchaseOrderItemDto(
     Guid ProductId,
     string ProductName,
     int QuantityOrdered,
     decimal UnitPrice
    );

    public record CreatePurchaseOrderDto(
    Guid SupplierId,
    List<CreatePurchaseOrderItemDto> Items,
    DateTime? ExpectedDeliveryDate,
    string? Notes,
    decimal Discount,
    decimal Tax);
}
