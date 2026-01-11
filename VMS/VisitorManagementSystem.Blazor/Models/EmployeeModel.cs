namespace VisitorManagementSystem.Blazor.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty; // added
        public bool IsAvailable { get; set; } = true;         // added
    }
}
