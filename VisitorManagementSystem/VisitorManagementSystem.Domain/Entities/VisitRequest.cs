using VisitorManagementSystem.Domain.Enums;

namespace VisitorManagementSystem.Domain.Entities
{
    public class VisitRequest
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }        // Reference to Visitor
        public int EmployeeId { get; set; }       // Reference to Employee
        public string? Reason { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.Now;
        public VisitStatus Status { get; set; } = VisitStatus.Pending;
        public int? ApprovedBy { get; set; }      // Employee Id who approved
        public DateTime? ApprovedAt { get; set; }
    }
}
