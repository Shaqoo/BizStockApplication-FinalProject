using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CustomerType
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public CustomerTypeName TypeName { get; private set; }

        public string? Description { get; private set; }

        public decimal? DiscountPercentage { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<Customer> Customers { get; private set; } = new List<Customer>();

        private CustomerType() { }

        public CustomerType(CustomerTypeName typeName, string? description = null, decimal? discount = null)
        {
            TypeName = typeName;
            Description = description;
            DiscountPercentage = discount;
        }

        public void UpgradeToVip()
        {
            TypeName = CustomerTypeName.VIP;
        }

    }

}
