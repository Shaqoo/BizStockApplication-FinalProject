using Application.Dto;
using Application.Interfaces.Service;
using Domain.Enums;
using Host.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;

namespace Host.Service
{
    public class Notifier : INotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        private readonly IHubContext<ChatMessageHub> _chatHubContext;
        public Notifier(IHubContext<NotificationHub> hubContext,IHubContext<ChatMessageHub> chatHubContext)
        {
            _hubContext = hubContext;
            _chatHubContext = chatHubContext;
        }

        public async Task SendNotificationAsync(Guid userId, NotificationDto message)
        {
            await _hubContext.Clients
                .Group(userId.ToString())
                .SendAsync("ReceiveNotification", message);
        }

        public async Task BroadcastToCustomerServiceAsync(NotificationDto message)
        {
            await _hubContext.Clients
                .Group("CustomerServiceAgents")
                .SendAsync("ReceiveNotification", message);
        }

        public async Task SendChatMessageAsync(MessageDto message, Guid threadId)
        {
            await _chatHubContext.Clients
                .Group(threadId.ToString())
                .SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageReadAsync(Guid chatThreadId, Guid messageId, Guid readerId)
        {
            await _chatHubContext.Clients
                .Group(chatThreadId.ToString())
                .SendAsync("MessageRead", new
                {
                    MessageId = messageId,
                    ReaderId = readerId
                });
        }

        public async Task SendMessageReactionAsync(Guid chatThreadId, Guid messageId, Guid userId, string emoji)
        {
            await _chatHubContext.Clients.Group(chatThreadId.ToString()).SendAsync("MessageReactionReceived", new
            {
                MessageId = messageId,
                UserId = userId,
                Emoji = emoji
            });
        }

        public async Task SendToRoleAsync(Role role, NotificationDto message)
        {
            await _hubContext.Clients.Group(role.ToString())
           .SendAsync("ReceiveNotification",message);
        }

        public async Task NotifyNotificationReadAsync(Guid userId, Guid notificationId)
        {
            await _hubContext.Clients.Group(userId.ToString())
                .SendAsync("NotificationMarkedAsRead", new
                {
                    NotificationId = notificationId
                });
        }

        public async Task NotifyAllNotificationsReadAsync(Guid userId)
        {
            await _hubContext.Clients.Group(userId.ToString())
                .SendAsync("AllNotificationsMarkedAsRead");
        }
    }
}
