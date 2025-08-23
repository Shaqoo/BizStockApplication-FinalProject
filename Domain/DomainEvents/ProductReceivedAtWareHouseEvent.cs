using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public sealed class ProductReceivedAtWarehouseEvent : INotification
    {
        public Guid PurchaseOrderId { get; init; }
        public Guid ProductId { get; init; }
        public int QuantityReceived { get; init; }
        public Guid WarehouseId { get; init; }
        public DateTime ReceivedAt { get; init; }
    }

}
