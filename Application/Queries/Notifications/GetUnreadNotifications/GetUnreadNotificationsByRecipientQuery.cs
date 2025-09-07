using Application.Dto;
using MediatR;

namespace Application.Queries.Notifications.GetUnreadNotifications
{
    public record GetUnreadNotificationsByRecipientQuery()
        : IRequest<Result<IEnumerable<NotificationDto>>>;

}
