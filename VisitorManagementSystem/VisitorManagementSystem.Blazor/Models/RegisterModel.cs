namespace VisitorManagementSystem.Blazor.Models
{
    public class RegisterModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // Admin or User
    }
}