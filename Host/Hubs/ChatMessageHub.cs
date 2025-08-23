using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Host.Hubs
{
    [Authorize]
    public class ChatMessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            
            await base.OnDisconnectedAsync(exception);
        }
        public async Task JoinThread(Guid threadId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, threadId.ToString());
        }

        public async Task LeaveThread(Guid threadId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, threadId.ToString());
        }

    }
}
