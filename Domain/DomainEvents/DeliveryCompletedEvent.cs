using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class DeliveryCompletedEvent : INotification
    {
        public Guid AssignmentId { get; init; }
        public Guid SalesOrderId { get; init; }
        public Guid DeliveryAgentId { get; init; }
        public DateTime DeliveredAt { get; init; }
    }

}
