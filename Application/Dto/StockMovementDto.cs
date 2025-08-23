using Domain.Enums;

namespace Application.Dto
{
    public record StockMovementDto(Guid Id,Guid ProductId,StockMovementType MovementType,
        int QuantityChanged,Guid WarehouseId,DateTimeOffset Date,Guid? UserId,string? Reason);
     
}
