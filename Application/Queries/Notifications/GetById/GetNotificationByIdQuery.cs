using Application.Dto;
using MediatR;

namespace Application.Queries.Notifications.GetById
{
    public record GetNotificationByIdQuery(Guid Id) : IRequest<Result<NotificationDto>>;

}
