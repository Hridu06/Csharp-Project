using System;

namespace VisitorManagementSystem.Application.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "VisitRequest";
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public int? VisitRequestId { get; set; }

        // Optional display-only fields
        public string? SenderName { get; set; }
        public string? ReceiverName { get; set; }
    }
}
