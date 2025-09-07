using Application.Dto;
using MediatR;

namespace Application.Commands.Notifications.MarkAllAsRead
{
    public record MarkAllNotificationsAsReadCommand() : IRequest<Result<string>>;

}
