namespace VisitorManagementSystem.Blazor.Models
{
    public class RegisterModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // "Admin" or "User"
    }

    public class LoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class AuthResultModel
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? UserId { get; set; }      // Identity User Id
        public string? FullName { get; set; }
        public int? EmployeeId { get; set; }
    }
}
