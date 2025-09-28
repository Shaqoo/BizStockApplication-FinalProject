using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace Host.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user?.FindFirst("Role")?.Value;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(role))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                if (role == "CustomerService")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "CustomerServiceAgents");
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, role);

                Console.WriteLine("SignalR {0}", role);
                Console.WriteLine("SignalR {0}",userId);
            }

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = Context.User?.FindFirst("Role")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, role);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
