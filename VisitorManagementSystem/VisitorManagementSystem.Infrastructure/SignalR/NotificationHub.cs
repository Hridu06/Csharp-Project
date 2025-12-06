using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Infrastructure.SignalR
{
    // This hub handles notifications between Visitors and Employees
    public class NotificationHub : Hub
    {
        // Called when a visitor sends a visit request
        public async Task SendVisitorRequest(string employeeId, string visitorName)
        {
            // Send the visitor request to the specific employee
            await Clients.User(employeeId).SendAsync("ReceiveVisitorRequest", visitorName);
        }

        // Called when employee approves or declines a visitor
        public async Task SendVisitorRequestStatus(string visitorId, string status)
        {
            await Clients.User(visitorId).SendAsync("ReceiveRequestStatus", status);
        }

        // Optional: override OnConnected to log or handle user connection
        public override Task OnConnectedAsync()
        {
            // Optionally map connectionId to userId if needed
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            // Handle disconnect logic if needed
            return base.OnDisconnectedAsync(exception);
        }
    }
}
