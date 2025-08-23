using Application.Dto;
using Domain.Enums;

namespace Application.Interfaces.Service
{
    public interface INotifier
    {
        Task SendNotificationAsync(Guid userId, NotificationDto message);
        Task SendToRoleAsync(Role role, NotificationDto message);
        Task NotifyNotificationReadAsync(Guid userId, Guid notificationId);
        Task NotifyAllNotificationsReadAsync(Guid userId);
        Task BroadcastToCustomerServiceAsync(NotificationDto message);
        Task SendChatMessageAsync(MessageDto message, Guid threadId);
        Task SendMessageReadAsync(Guid chatThreadId, Guid messageId, Guid readerId);
        Task SendMessageReactionAsync(Guid chatThreadId, Guid messageId, Guid userId, string emoji);

    }
}
