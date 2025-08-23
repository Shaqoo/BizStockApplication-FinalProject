using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Notifications.SendNotificationViaRoles
{
    public record SendNotificationToRoleCommand(NotificationRequest Request)
        : IRequest<Result<string>>;

}
