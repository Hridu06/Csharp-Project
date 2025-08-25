using BMS.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Shop> Shops => Set<Shop>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<BookShop> BookShops => Set<BookShop>();
        public DbSet<Order> Orders => Set<Order>();
    }
}
