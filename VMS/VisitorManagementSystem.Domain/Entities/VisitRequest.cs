using VisitorManagementSystem.Domain.Entities;

public class VisitRequest
{
    public int Id { get; set; }                     // Primary Key
    public string VisitorId { get; set; } = "";     // FK -> Identity User
    public int EmployeeId { get; set; }             // FK -> Employee
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? Visitor { get; set; }    // Navigation
    public Employee? Employee { get; set; }
}
