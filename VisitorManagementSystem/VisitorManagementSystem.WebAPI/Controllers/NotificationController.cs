using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.WebAPI.Hubs;

namespace VisitorManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _authService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(
            INotificationService notificationService,
            IUserService authService,
            IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _authService = authService;
            _hubContext = hubContext;
        }

        // ✅ Visitor sends notification to Employee
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto notificationDto)
        {
            if (notificationDto == null)
                return BadRequest("Invalid notification data.");

            // Sender = current logged-in visitor
            notificationDto.SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create notification
            var createdNotification = await _notificationService.SendNotificationAsync(notificationDto);

            // SignalR push to Employee (Receiver)
            await _hubContext.Clients.User(notificationDto.ReceiverId)
                             .SendAsync("ReceiveNotification", createdNotification);

            return Ok(createdNotification);
        }

        // ✅ Get all notifications for the logged-in Employee or Admin
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetNotifications()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized("Invalid token: email not found.");

            var employeeId = await _authService.GetEmployeeIdByEmailAsync(email);
            if (employeeId == null)
                return BadRequest("Employee not found for this account.");

            var notifications = await _notificationService.GetNotificationsForEmployeeAsync(employeeId.Value);
            return Ok(notifications);
        }

        // ✅ Mark notification as read
        [HttpPut("{id}/read")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await _notificationService.MarkAsReadAsync(id);
            if (!success) return NotFound("Notification not found.");
            return Ok("Notification marked as read.");
        }

        // ✅ Admin deletes a notification
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var success = await _notificationService.DeleteNotificationAsync(id);
            if (!success) return NotFound("Notification not found.");
            return Ok("Notification deleted successfully.");
        }
    }
}
