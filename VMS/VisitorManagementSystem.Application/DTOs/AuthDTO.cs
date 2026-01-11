namespace VisitorManagementSystem.Application.DTOs
{
    public class RegisterDTO
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }  // "Admin" or "User"
    }

    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class AuthResponseDTO
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }

        // ✅ Add these two
        public string? UserId { get; set; }      // This will hold IdentityUser.Id
        public string? FullName { get; set; }    // Visitor's or user's full name
        public int? EmployeeId { get; set; }
    }
}
