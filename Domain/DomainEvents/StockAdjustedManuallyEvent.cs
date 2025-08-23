using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class StockAdjustedManuallyEvent : INotification
    {
        public Guid ProductId { get; init; }
        public Guid WarehouseId { get; init; }
        public string ProductName { get; init; } = default!;
        public string WarehouseName { get; init; } = default!;
        public int QuantityChanged { get; init; }
        public int FinalQuantity { get; init; }
        public string Reason { get; init; } = default!;
        public string PerformedBy { get; init; } = default!;
        public DateTime AdjustedAt { get; init; } = DateTime.UtcNow;
    }

}
