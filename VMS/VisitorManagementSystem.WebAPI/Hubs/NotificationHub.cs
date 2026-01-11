using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VisitorManagementSystem.WebAPI.Hubs
{
    public class NotificationHub : Hub
    {
        // ✅ Called when a client connects
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Assumes UserIdentifier is set to user ID
            if (!string.IsNullOrEmpty(userId))
            {
                // Add connection to a group for this user
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        // ✅ Called when a client disconnects
        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Optional: method for sending notification from server to specific user
        public async Task SendNotificationToUser(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
