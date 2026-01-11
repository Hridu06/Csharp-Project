using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VisitorManagementSystem.WebAPI.Hubs
{
    public class VisitorHub : Hub
    {
        // Join a specific employee group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Send notification to specific employee
        public async Task NotifyEmployee(string groupName, string visitorName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveVisitorNotification", visitorName);
        }
    }
}
