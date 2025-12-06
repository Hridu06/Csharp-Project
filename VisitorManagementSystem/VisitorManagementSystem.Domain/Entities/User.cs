using System.Data;
using VisitorManagementSystem.Domain.Enums;

namespace VisitorManagementSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public Role Role { get; set; }  // Admin, Employee, Visitor
    }
}
