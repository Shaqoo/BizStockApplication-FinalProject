using Domain.Auditable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DeliveryFeeRule
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = default!;         
        public string Zone { get; private set; } = default!;         
        public decimal FlatRate { get; private set; }                
        public decimal? MinOrderAmountForFree { get; private set; }   
        public bool IsActive { get; private set; } = true;
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;
        public string? Note { get; private set; }

        private DeliveryFeeRule() { }

        public DeliveryFeeRule(string name, string zone, decimal flatRate, decimal? minOrderAmountForFree = null, string? note = null)
        {
            Name = name;
            Zone = zone;
            FlatRate = flatRate;
            MinOrderAmountForFree = minOrderAmountForFree;
            Note = note;
        }

        public decimal CalculateFee(decimal orderTotal)
        {
            if (MinOrderAmountForFree.HasValue && orderTotal >= MinOrderAmountForFree.Value)
                return 0;

            return FlatRate;
        }

        public void Deactivate() => IsActive = false;
    }

}
