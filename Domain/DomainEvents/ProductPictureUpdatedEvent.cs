using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public record ProductPictureUpdatedEvent(
    Guid ProductId,
    string ProductName,
    Guid UpdatedByUserId
) : INotification;

}
