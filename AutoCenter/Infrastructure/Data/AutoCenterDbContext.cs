using AutoCenter.Web.Enums;
using AutoCenter.Web.Infrastructure.Data.Seed;
using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace AutoCenter.Web.Infrastructure.Data
{
    public class AutoCenterDbContext:IdentityDbContext
    {
        public AutoCenterDbContext(DbContextOptions<AutoCenterDbContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Brand>().Property(b => b.Name).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Brand>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<Brand>().HasMany(a=>a.CarModels).WithOne(b=>b.Brand).HasForeignKey(b=>b.BrandId);
            modelBuilder.Entity<CarModel>().HasIndex(cm => new { cm.BrandId, cm.Name }).IsUnique();
        }
        //public DbSet<User> Users { get; set; }
        //public DbSet<AgencyUser> AgencyUsers { get; set; } 
        public DbSet<Listing> Listings => Set<Listing>();
        public DbSet<Brand> CarBrands => Set<Brand>();

    }
}
