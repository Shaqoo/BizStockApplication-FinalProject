using MediatR;

namespace Domain.DomainEvents
{
    public record MfaResetEvent(
    Guid UserId,
    string Email,
    string FullName,
    DateTime ResetAtUtc
    ):INotification;
}
