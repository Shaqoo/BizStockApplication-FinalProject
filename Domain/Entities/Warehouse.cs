using Domain.Auditable;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Location { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;

        public ICollection<WarehouseItem> Items { get; private set; } = new List<WarehouseItem>();

        private Warehouse() { }

        public Warehouse(string name, string location)
        {
            SetName(name);
            SetLocation(location);
        }

        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                Modified();
            }
        }

        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
                Modified();
            }
        }

        public void Update(string name, string location)
        {
            SetName(name);
            SetLocation(location);
            Modified();
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Warehouse name is required.");
            Name = name.Trim();
        }

        private void SetLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new DomainException("Warehouse location is required.");
            Location = location.Trim();
        }
    }


}
