using Application.Dto;
using MediatR;

namespace Application.Queries.Notifications.GetUnreadNotifications
{
    public record GetUnreadNotificationsByRecipientQuery(Guid RecipientId)
        : IRequest<Result<IEnumerable<NotificationDto>>>;

}
