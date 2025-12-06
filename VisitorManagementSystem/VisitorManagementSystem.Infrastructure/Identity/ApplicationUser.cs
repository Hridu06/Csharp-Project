using Microsoft.AspNetCore.Identity;
using VisitorManagementSystem.Domain.Enums;

namespace VisitorManagementSystem.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public Role Role { get; set; }   // Enum: Admin, Employee, Visitor
    }
}
