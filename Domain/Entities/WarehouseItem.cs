using Domain.Auditable;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class WarehouseItem : BaseEntity
    {
        public Guid WarehouseId { get; private set; }
        public Warehouse Warehouse { get; private set; } = default!;
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public int ReorderLevel { get; private set; }               
        public int Quantity { get; private set; }                   

        private WarehouseItem() { }

        public WarehouseItem(Guid warehouseId, Guid productId, int reorderLevel, int quantity)
        {
            if (quantity < 0) throw new DomainException("Quantity cannot be negative.");
            if (reorderLevel < 0) throw new DomainException("Reorder level cannot be negative.");

            WarehouseId = warehouseId;
            ProductId = productId;
            ReorderLevel = reorderLevel;
            Quantity = quantity;
        }

        public void IncreaseStock(int amount)
        {
            if (amount <= 0) throw new DomainException("Amount must be positive.");
            Quantity += amount;
            Modified();
        }

        public void DecreaseStock(int amount)
        {
            if (amount <= 0 || amount > Quantity)
                throw new DomainException("Insufficient stock.");
            Quantity -= amount;
            Modified();
        }

        public void SetReorderLevel(int level)
        {
            if (level < 0) throw new DomainException("Reorder level cannot be negative.");
            ReorderLevel = level;
            Modified();
        }

        public bool NeedsReordering() => Quantity <= ReorderLevel;
    }


}
