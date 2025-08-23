using Domain.Auditable;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public Guid WarehouseItemId { get; private set; }
        public WarehouseItem WarehouseItem { get; private set; } = default!;
        public StockMovementType MovementType { get; private set; } 
        public int QuantityChanged { get; private set; }  
        public string? Reason { get; private set; } = default!;
        public Guid? PerformedByUserId { get; private set; }  
        public User? PerformedByUser { get; private set; }

        private StockMovement() { }

        public StockMovement(
            Guid warehouseItemId,
            StockMovementType movementType,
            int quantityChanged,
            string? reason,
            Guid? performedByUserId = null
        )
        {
            if (quantityChanged == 0)
                throw new DomainException("Quantity changed must not be zero.");

            WarehouseItemId = warehouseItemId;
            MovementType = movementType;
            QuantityChanged = quantityChanged;
            Reason = reason;
            PerformedByUserId = performedByUserId;
        }
    }

}
