using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class PaymentReceivedEvent : INotification
    {
        public Guid PaymentId { get; init; }
        public Guid InvoiceId { get; init; }
        public decimal Amount { get; init; }
        public DateTime PaidAt { get; init; }
    }

}
