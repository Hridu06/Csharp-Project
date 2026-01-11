using System;

namespace VisitorManagementSystem.Application.DTOs
{
    public class VisitRequestDto
    {
        public int Id { get; set; }                         // DB primary key
        public string VisitorId { get; set; } = "";          // string (Identity user)
        public int EmployeeId { get; set; }                  // int (Employee table)
        public string VisitorName { get; set; } = "";        // extra info
        public string EmployeeName { get; set; } = "";       // extra info
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
