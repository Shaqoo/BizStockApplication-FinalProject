using Domain.Enums;

namespace Application.Dto
{
    public record WarehouseProductDto(Guid ProductId,Guid WarehouseItem,int Quantity,int Reorderlevel,string ProductName,
        string WarehouseName,string ProductPictureUrl,string Location,string Sku,UnitOfMeasure UnitOfMeasure);
   
}
