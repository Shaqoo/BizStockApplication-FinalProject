using Domain.Enums;

namespace Application.Dto.RequestModels
{
    public record AdjustStockRequest(Guid ProductId,
        Guid WarehouseId,
        int Quantity,
        AdjustmentType AdjustmentType,
        string Reason
    );

}
