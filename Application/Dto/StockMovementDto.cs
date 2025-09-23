using Domain.Enums;

namespace Application.Dto
{
    public record StockMovementDto(Guid Id,Guid ProductId,StockMovementType MovementType,
        int QuantityChanged,Guid WarehouseId,DateTimeOffset Date,Guid? UserId,string? Reason);

    public record StockMovementStatsDto
(
    int TotalInbound,
    int TotalOutbound,
    int TotalAdjustmentIn,
    int TotalAdjustmentOut,
    int TotalTransferIn,
    int TotalTransferOut,
    int TotalMovements
);

    public record StockMovementTrendDto
    {
        public string Period { get; set; } = string.Empty; 
        public int Inbound { get; set; }
        public int Outbound { get; set; }
        public int AdjustmentIn { get; set; }
        public int AdjustmentOut { get; set; }
        public int TransferIn { get; set; }
        public int TransferOut { get; set; }
    }


}
