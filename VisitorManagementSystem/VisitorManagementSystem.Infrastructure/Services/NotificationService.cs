using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using VisitorManagementSystem.Infrastructure.SignalR;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // Notify employee that a visitor requested a visit
        public async Task SendVisitorRequestNotificationAsync(string employeeId, string visitorName)
        {
            await _hubContext.Clients.User(employeeId)
                .SendAsync("ReceiveVisitorRequest", visitorName);
        }

        // Notify visitor that request was approved or declined
        public async Task SendVisitorRequestStatusAsync(string visitorId, string status)
        {
            await _hubContext.Clients.User(visitorId)
                .SendAsync("ReceiveRequestStatus", status);
        }
    }
}
