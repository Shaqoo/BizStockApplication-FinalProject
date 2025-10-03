using Domain.Auditable;
using Domain.Enums;

namespace Domain.Entities
{
    public class DeliveryAssignment : BaseEntity
    {
        public Guid SalesOrderId { get; private set; }
        public SalesOrder SalesOrder { get; private set; } = default!;
        public Guid? DeliveryAgentId { get; private set; }
        public DeliveryAgent? DeliveryAgent { get; private set; }
        public string RecipientName { get; private set; } = default!;
        public string RecipientPhone { get; private set; } = default!;
        public string? RecipientEmail { get; private set; }
        public bool IsExternal { get; private set; } = false;
        public string? ExternalDeliveryService { get; private set; }
        public string? ExternalJobId { get; private set; }         
        public decimal DeliveryFee { get; private set; }
        public DeliveryStatus Status { get; private set; } = DeliveryStatus.Pending;
        public DateTime? DeliveredAt { get; private set; }
        public string? Note { get; private set; }
        public DeliveryAddress DeliveryAddress { get; private set; } = default!;
        public Guid DeliveryAddressId { get; private set; }

        private DeliveryAssignment() { }

        public DeliveryAssignment(Guid salesOrderId, decimal deliveryFee, Guid? deliveryAgentId, string? note = null)
        {
            SalesOrderId = salesOrderId;
            DeliveryAgentId = deliveryAgentId;
            DeliveryFee = deliveryFee;
            Note = note;
            Status = DeliveryStatus.Pending;
            IsExternal = false;
        }

        public DeliveryAssignment(Guid salesOrderId,Guid deliveryAddressId, decimal deliveryFee, string externalJobId,string email,string phone,string name, string externalService = "Faz", string? note = null)
        {
            SalesOrderId = salesOrderId;
            DeliveryAddressId = deliveryAddressId;
            DeliveryFee = deliveryFee;
            ExternalJobId = externalJobId;
            ExternalDeliveryService = externalService;
            Note = note;
            Status = DeliveryStatus.Pending;
            IsExternal = true;
            RecipientEmail = email;
            RecipientPhone = phone;
            RecipientName = name;
        }

        public void MarkAsInTransit()
        {
            Status = DeliveryStatus.InTransit;
            Modified();
        }

        public void MarkAsDelivered()
        {
            Status = DeliveryStatus.Delivered;
            DeliveredAt = DateTime.UtcNow;
            Modified();
        }

        public void MarkAsFailed(string? note = null)
        {
            Status = DeliveryStatus.Failed;
            Note = note ?? Note;
            Modified();
        }
    }

}
