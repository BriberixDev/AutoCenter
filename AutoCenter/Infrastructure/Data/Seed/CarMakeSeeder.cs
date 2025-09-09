using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Infrastructure.Data.Seed
{
    public class CarMakeSeeder
    {
        private readonly AutoCenterDbContext _context;
        public CarMakeSeeder (AutoCenterDbContext autoCenterDbContext)
        {
            _context = autoCenterDbContext;
        }

        public static readonly List<string> BrandNames = new()
        {
            "Audi",
            "BMW",
            "Mercedes-Benz",
            "Porsche"
        };
        public async Task SeedAsync()
        {
            var existing = await _context.CarBrands
               .Select(m => m.Name)
               .ToListAsync();

            var newMakes = BrandNames
                .Where(m => !existing.Contains(m))
                .Select(m => new Brand { Name = m })
                .ToList();

            if (newMakes.Any())
                {
                await _context.CarBrands.AddRangeAsync(newMakes); //Add all changes to AutoCenterDbContext
                await _context.SaveChangesAsync();
            }
        }
        
    }
}
