using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VisitorManagementSystem.Domain.Enums;
using VisitorManagementSystem.Infrastructure.Data;
using VisitorManagementSystem.Infrastructure.Identity;
using VisitorManagementSystem.Infrastructure.Repositories;
using VisitorManagementSystem.Infrastructure.Services;
using VisitorManagementSystem.Infrastructure.SignalR;

namespace VisitorManagementSystem.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            // -------------------------------
            // 1️⃣ ApplicationDbContext (Identity / EF Core)
            // -------------------------------
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity configuration
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // -------------------------------
            // 2️⃣ DapperContext
            // -------------------------------
            services.AddSingleton<DapperContext>();

            // -------------------------------
            // 3️⃣ Repositories (Dapper)
            // -------------------------------
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<VisitorRepository>();
            services.AddScoped<VisitRequestRepository>();

            // -------------------------------
            // 4️⃣ Services
            // -------------------------------
            services.AddScoped<IdentityService>();
            services.AddScoped<AuthService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<VisitorApprovalService>();

            // -------------------------------
            // 5️⃣ SignalR
            // -------------------------------
            services.AddSignalR();

            return services;
        }
    }
}
