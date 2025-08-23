using Application.Dto;
using MediatR;

namespace Application.Commands.Notifications.MarkAllAsRead
{
    public record MarkAllNotificationsAsReadCommand(Guid RecipientId) : IRequest<Result<string>>;

}
