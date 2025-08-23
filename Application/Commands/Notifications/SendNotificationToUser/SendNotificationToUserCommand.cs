using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Notifications.SendNotificationToUser
{
    public record SendNotificationToUserCommand(NotificationRequest Request)
    : IRequest<Result<string>>;

}
