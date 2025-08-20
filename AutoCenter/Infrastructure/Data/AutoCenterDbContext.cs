using AutoCenter.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Infrastructure.Data
{
    public class AutoCenterDbContext:DbContext
    {
        public AutoCenterDbContext(DbContextOptions<AutoCenterDbContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<AgencyUser> AgencyUsers { get; set; } 
        public DbSet<Listing> Listings { get; set; } 
        public DbSet<Vehicle> Vehicles { get; set; } 
    }
}
