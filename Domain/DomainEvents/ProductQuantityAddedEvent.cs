using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public record ProductQuantityAddedEvent(
    Guid ProductId,
    string ProductName,
    int AddedQuantity,
    Guid AddedByUserId
) : INotification;

}
