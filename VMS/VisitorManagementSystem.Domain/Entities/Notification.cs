using System;
using VisitorManagementSystem.Domain.Entities;

namespace VisitorManagementSystem.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }                              // Primary Key
        public string SenderId { get; set; } = string.Empty;      // FK -> User (Visitor/Admin)
        public string ReceiverId { get; set; } = string.Empty;    // FK -> User (Employee)
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "VisitRequest";        // e.g. VisitRequest, System, Alert
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? VisitRequestId { get; set; }                  // Optional FK

        // Navigation Properties
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
        public VisitRequest? VisitRequest { get; set; }
    }
}
