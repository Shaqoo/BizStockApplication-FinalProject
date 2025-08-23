using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class CustomerUpgradedToVipEvent : INotification
    {
        public Guid CustomerId { get; init; }
        public string FullName { get; init; } = default!;
        public DateTime UpgradedAt { get; init; }
    }

}
