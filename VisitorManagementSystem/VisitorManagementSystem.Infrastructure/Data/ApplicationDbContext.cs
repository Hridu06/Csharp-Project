using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisitorManagementSystem.Infrastructure.Identity;
using VisitorManagementSystem.Domain.Entities;

namespace VisitorManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // These DbSets are defined for potential EF Core operations (mainly for Identity relations or admin use)
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<VisitRequest> VisitRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relationships and constraints configuration

            builder.Entity<VisitRequest>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(v => v.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VisitRequest>()
                .HasOne<Visitor>()
                .WithMany()
                .HasForeignKey(v => v.VisitorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
