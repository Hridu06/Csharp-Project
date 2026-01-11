using System.Collections.Generic;

namespace VisitorManagementSystem.Blazor.Models
{
    public class AuthResultModel
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public string? Message { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        // ✅ Add these two fields
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public int? EmployeeId { get; set; } // ✅ For employees
    }
}
