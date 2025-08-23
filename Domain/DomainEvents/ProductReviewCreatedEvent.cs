using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public record ProductReviewCreatedEvent(
    Guid ProductId,
    string ProductName,
    string CreatedByUser
) : INotification;

}
