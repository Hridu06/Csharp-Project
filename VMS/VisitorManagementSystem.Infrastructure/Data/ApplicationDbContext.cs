using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisitorManagementSystem.Domain.Entities;

namespace VisitorManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<VisitRequest> VisitRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; } // ✅ Add Notifications

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ Unique email for Employees (prevents duplicates)
            builder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // ✅ VisitRequest → Employee (many-to-one)
            builder.Entity<VisitRequest>()
                .HasOne(v => v.Employee)
                .WithMany(e => e.VisitRequests)
                .HasForeignKey(v => v.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ VisitRequest → Visitor (User table)
            builder.Entity<VisitRequest>()
                .HasOne(v => v.Visitor)
                .WithMany()
                .HasForeignKey(v => v.VisitorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Notification → Sender (User)
            builder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany()
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Notification → Receiver (User)
            builder.Entity<Notification>()
                .HasOne(n => n.Receiver)
                .WithMany()
                .HasForeignKey(n => n.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Notification → VisitRequest (optional)
            builder.Entity<Notification>()
                .HasOne(n => n.VisitRequest)
                .WithMany()
                .HasForeignKey(n => n.VisitRequestId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
