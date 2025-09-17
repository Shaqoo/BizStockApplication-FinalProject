using MediatR;

namespace Domain.DomainEvents
{
    public record ChatThreadClosedEvent(Guid ThreadId, Guid AgentId,Guid CustomerId) : INotification;

}
