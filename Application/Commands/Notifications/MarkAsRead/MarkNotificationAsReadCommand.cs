using Application.Dto;
using MediatR;

namespace Application.Commands.Notifications.MarkAsRead
{
    public record MarkNotificationAsReadCommand(Guid NotificationId) : IRequest<Result<string>>;

}
