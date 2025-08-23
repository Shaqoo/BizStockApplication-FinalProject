using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public sealed class ProductStockLowEvent : INotification
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; } = default!;
        public int CurrentStock { get; init; }
        public int ReorderLevel { get; init; }
    }

}
