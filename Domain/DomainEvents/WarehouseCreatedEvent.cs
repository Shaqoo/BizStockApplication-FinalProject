using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class WarehouseCreatedEvent : INotification
    {
        public Guid WarehouseId { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public WarehouseCreatedEvent(Guid Id,string Name,string Location)
        {
            WarehouseId = Id;
            this.Name = Name;
            this.Location = Location;
        }

    }
}
