namespace Application.Dto
{
    public record ProductStockSummaryDto(
    Guid ProductId,
    string ProductName,
    int TotalQuantity,
    List<WarehouseStockDto> Warehouses
);

    public record WarehouseStockDto(
        Guid WarehouseId,
        string WarehouseName,
        int Quantity
    );

}
