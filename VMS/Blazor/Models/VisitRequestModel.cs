using System;

namespace VisitorManagementSystem.Blazor.Models
{
    public class VisitRequestModel
    {
        public int Id { get; set; }
        public string VisitorId { get; set; } = "";
        public int EmployeeId { get; set; }
        public string VisitorName { get; set; } = "";
        public string EmployeeName { get; set; } = "";
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
