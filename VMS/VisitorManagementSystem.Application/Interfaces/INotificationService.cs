using VisitorManagementSystem.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Application.Interfaces
{
    public interface INotificationService
    {
        // Send a notification to a specific employee
        Task<NotificationDto> SendNotificationAsync(NotificationDto notificationDto);

        // Get all notifications for a specific employee
        Task<IEnumerable<NotificationDto>> GetNotificationsForEmployeeAsync(int employeeId);

        // Mark a notification as read
        Task<bool> MarkAsReadAsync(int notificationId);

        // Optional: Delete a notification
        Task<bool> DeleteNotificationAsync(int notificationId);
    }
}
