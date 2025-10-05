using Domain.Enums;

namespace Application.Dto
{
    public class DeliveryAssignmentDto
    {
        public Guid Id { get; set; }

        public Guid SalesOrderId { get; set; }

        public Guid? DeliveryAgentId { get; set; }
        public string? DeliveryAgentName { get; set; }
        public string? DeliveryAgentPhone { get; set; }

        public string RecipientName { get; set; } = default!;
        public string RecipientPhone { get; set; } = default!;
        public string? RecipientEmail { get; set; }

        public bool IsExternal { get; set; }
        public string? ExternalDeliveryService { get; set; }
        public string? ExternalJobId { get; set; }

        public decimal DeliveryFee { get; set; }

        public DeliveryStatus Status { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? Note { get; set; }

        public Guid DeliveryAddressId { get; set; } = default!;
    }

}
