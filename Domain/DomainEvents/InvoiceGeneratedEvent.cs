using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class InvoiceGeneratedEvent : INotification
    {
        public Guid InvoiceId { get; init; }
        public Guid SalesOrderId { get; init; }
        public Guid CustomerId { get; init; }
        public decimal Amount { get; init; }
        public DateTime GeneratedAt { get; init; }
    }

}
