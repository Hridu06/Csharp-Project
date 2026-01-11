using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorManagementSystem.Application.DTOs;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Domain.Entities;
using VisitorManagementSystem.Infrastructure.Data;

namespace VisitorManagementSystem.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Send a notification
        public async Task<NotificationDto> SendNotificationAsync(NotificationDto notificationDto)
        {
            var notification = new Notification
            {
                SenderId = notificationDto.SenderId,
                ReceiverId = notificationDto.ReceiverId,
                Message = notificationDto.Message,
                Type = notificationDto.Type ?? "VisitRequest",
                CreatedAt = DateTime.UtcNow,
                VisitRequestId = notificationDto.VisitRequestId,
                IsRead = false
            };

            _context.Set<Notification>().Add(notification);
            await _context.SaveChangesAsync();

            // Return DTO with generated Id
            notificationDto.Id = notification.Id;
            notificationDto.CreatedAt = notification.CreatedAt;

            return notificationDto;
        }

        // Get notifications for an employee
        public async Task<IEnumerable<NotificationDto>> GetNotificationsForEmployeeAsync(int employeeId)
        {
            string receiverId = employeeId.ToString();

            var notifications = await _context.Set<Notification>()
                .Where(n => n.ReceiverId == receiverId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    SenderId = n.SenderId,
                    ReceiverId = n.ReceiverId,
                    Message = n.Message,
                    Type = n.Type,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    VisitRequestId = n.VisitRequestId
                })
                .ToListAsync();

            return notifications;
        }

        // Mark notification as read
        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Set<Notification>().FindAsync(notificationId);
            if (notification == null) return false;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        // Optional: delete notification
        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Set<Notification>().FindAsync(notificationId);
            if (notification == null) return false;

            _context.Set<Notification>().Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
