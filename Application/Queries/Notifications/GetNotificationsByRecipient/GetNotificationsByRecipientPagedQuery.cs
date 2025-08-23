using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Notifications.GetNotificationsByRecipient
{
    public record GetNotificationsByRecipientPagedQuery(Guid RecipientId, PageRequest PageRequest)
    : IRequest<Result<PaginatedList<NotificationDto>>>;
}
