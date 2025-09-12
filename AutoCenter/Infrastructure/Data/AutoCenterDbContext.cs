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

            //Brand
            modelBuilder.Entity<Brand>()
                .Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(64);

            modelBuilder.Entity<Brand>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<Brand>()
                .HasMany(a=>a.CarModels)
                .WithOne(b=>b.Brand)
                .HasForeignKey(b=>b.BrandId)
                .OnDelete(DeleteBehavior.Restrict);


            //CarModel
            modelBuilder.Entity<CarModel>()
                .HasIndex(cm => new { cm.BrandId, cm.Name })
                .IsUnique();


            //VehicleSpec
            modelBuilder.Entity<VehicleSpec>()
                .HasKey(vs => vs.Id);

            modelBuilder.Entity<VehicleSpec>()
                .HasOne(vs => vs.Brand)
                .WithMany()
                .HasForeignKey(vs => vs.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleSpec>()
               .HasOne(vs => vs.CarModel)
               .WithMany()
               .HasForeignKey(vs => vs.CarModelId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleSpec>()
                .Property(vs => vs.Vin)
                .HasMaxLength(17);

            modelBuilder.Entity<VehicleSpec>()
                .Property(vs => vs.Year)
                .IsRequired()
                .HasMaxLength(4);

            //Listing
            modelBuilder.Entity<Listing>()
                .HasOne(l => l.Vehicle)
                .WithOne()
                .HasForeignKey<Listing>(l => l.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);


        }
        //public DbSet<User> Users { get; set; }
        //public DbSet<AgencyUser> AgencyUsers { get; set; } 
        public DbSet<Listing> Listings => Set<Listing>();
        public DbSet<VehicleSpec> VehicleSpecs => Set<VehicleSpec>();
        public DbSet<Brand> CarBrands => Set<Brand>();
        public DbSet<CarModel> CarModels => Set<CarModel>();

    }
}
