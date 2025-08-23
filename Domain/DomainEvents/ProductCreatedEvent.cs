using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{

    public sealed record ProductCreatedEvent(
        Guid ProductId,
        string Name,
        string SKU,
        Guid BrandId,
        Guid CreatedBy,
        DateTime CreatedAt
    ) : INotification;
}
