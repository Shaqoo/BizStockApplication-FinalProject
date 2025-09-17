using MediatR;

namespace Domain.DomainEvents
{
    public record ChatThreadAssignedEvent(Guid ThreadId, Guid AgentId) : INotification;
}
