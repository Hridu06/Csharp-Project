using VisitorManagementSystem.Domain.Enums;

namespace VisitorManagementSystem.Application.DTOs
{
    public class VisitRequestDto
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public int EmployeeId { get; set; }
        public string? Reason { get; set; }
        public DateTime RequestedAt { get; set; }
        public VisitStatus Status { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
