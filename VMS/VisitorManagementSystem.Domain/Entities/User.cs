using Microsoft.AspNetCore.Identity;

namespace VisitorManagementSystem.Domain.Entities
{
    public class User : IdentityUser
    {
        // Additional custom fields (optional)
        public string? FullName { get; set; }
        public string? Role { get; set; } // can be Admin/User etc.
    }
}
