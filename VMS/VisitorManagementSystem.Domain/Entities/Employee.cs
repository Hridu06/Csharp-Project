using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }                    // Primary Key
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        // Navigation property (optional)
        public ICollection<VisitRequest>? VisitRequests { get; set; }
    }
}
