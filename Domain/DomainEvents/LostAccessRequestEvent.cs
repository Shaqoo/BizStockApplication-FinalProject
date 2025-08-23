using MediatR;

namespace Domain.DomainEvents
{
    public record LostAccessRequestApprovedEvent(Guid RequestId, string UserIdentifier,
        string Fullname,string Notes,string Status,DateTimeOffset CreatedAt) : INotification;
    public record LostAccessRequestRejectedEvent(Guid RequestId, string UserIdentifier,
        string Fullname, string Notes, string Status, DateTimeOffset CreatedAt) : INotification;

}
