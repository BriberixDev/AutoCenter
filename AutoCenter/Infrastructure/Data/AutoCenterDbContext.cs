using AutoCenter.Web.Enums;
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
            modelBuilder.Entity<Listing>().OwnsOne(l => l.VehicleSpecs); //Configures the Listing entity to treat 'VehicleSpecs' as an owned entity
            modelBuilder.Entity<Listing>().OwnsOne(p => p.VehicleSpecs).HasData(
                new {
                    ListingId = 2, 
                    Make = "BMW",
                    Model = "320i",
                    Year = "2013",
                    Color = VehicleSpecsColor.Black,
                    Transmission = TransmissionType.Automatic,
                    FuelType = FuelType.Petrol,
                    BodyType = BodyType.Coupe,
                    Mileage = "150000",
                    Vin = "WBAVC31050KT12345"
                });
            modelBuilder.Entity<Listing>().HasData(
                new
                {
                    Id = 2,
                    Title = "BMW 320i Coupe",
                    Description = "A sleek and stylish coupe",
                    IsActive = true,
                    Price = 12000m
                });


        }
        //public DbSet<User> Users { get; set; }
        //public DbSet<AgencyUser> AgencyUsers { get; set; } 
        public DbSet<Listing> Listings => Set<Listing>();

    }
}
