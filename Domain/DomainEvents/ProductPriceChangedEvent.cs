using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public record ProductPriceChangedEvent(
    Guid ProductId,
    string ProductName,
    decimal OldPrice,
    decimal NewPrice,
    Guid ChangedByUserId
) : INotification;

}
