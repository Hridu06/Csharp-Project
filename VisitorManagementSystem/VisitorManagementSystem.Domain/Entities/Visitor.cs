using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorManagementSystem.Domain.Entities
{
    public class Visitor : User
    {
        public string? ContactNumber { get; set; }
    }
}

