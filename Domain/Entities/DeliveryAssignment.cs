using Domain.Auditable;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DeliveryAssignment : BaseEntity
    {
        public Guid SalesOrderId { get; private set; }
        public SalesOrder SalesOrder { get; private set; } = default!;
        public Guid DeliveryAgentId { get; private set; }
        public DeliveryAgent DeliveryAgent { get; private set; } = default!;
        public DateTime? DeliveredAt { get; private set; }

        public DeliveryStatus Status { get; private set; } = DeliveryStatus.Pending;

        public decimal DeliveryFee { get; private set; }

        public string? Note { get; private set; }

        private DeliveryAssignment() { }

        public DeliveryAssignment(Guid salesOrderId, Guid deliveryAgentId, decimal deliveryFee, string? note = null)
        {
            SalesOrderId = salesOrderId;
            DeliveryAgentId = deliveryAgentId;
            DeliveryFee = deliveryFee;
            Note = note;
            Status = DeliveryStatus.Pending;
        }

        public void MarkAsInTransit()
        {
            Status = DeliveryStatus.InTransit;
        }

        public void MarkAsDelivered()
        {
            Status = DeliveryStatus.Delivered;
            DeliveredAt = DateTime.UtcNow;
        }

        public void MarkAsFailed(string? note = null)
        {
            Status = DeliveryStatus.Failed;
            Note = note ?? Note;
        }
    }

}
