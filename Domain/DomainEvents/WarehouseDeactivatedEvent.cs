using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class WarehouseDeactivatedEvent : INotification
    {
        public Guid WarehouseId { get; init; }
        public string Name { get; init; } = default!;

        public string Location { get; init; } = default!;
        public WarehouseDeactivatedEvent(Guid Id,string Name,string Location)
        {
            WarehouseId = Id;
            this.Name = Name;
            this.Location = Location;
        }
    }
}
