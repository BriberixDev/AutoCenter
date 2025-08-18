using AutoCenter.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Infrastructure.Data
{
    public class AutoCenterDbContext:DbContext
    {
        public AutoCenterDbContext(DbContextOptions<AutoCenterDbContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }= null!;

        // Define DbSet properties for your entities here
        // public DbSet<User> Users { get; set; }
        // public DbSet<Vehicle> Vehicles { get; set; }
        // Add other DbSet properties as needed
        // You can also override OnConfiguring and OnModelCreating methods if necessary
    }
}
