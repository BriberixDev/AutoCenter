using AutoCenter.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Infrastructure.Data.Seed
{
    public class CarBrandSeeder
    {
        private readonly AutoCenterDbContext _context;
        public CarBrandSeeder (AutoCenterDbContext autoCenterDbContext)
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
            var existing = await _context.Brands
               .Select(m => m.Name)
               .ToListAsync();  

            var newMakes = BrandNames
                .Where(m => !existing.Contains(m))
                .Select(m => new Brand { Name = m })
                .ToList();

            if (newMakes.Any())
                {
                await _context.Brands.AddRangeAsync(newMakes); //Add all changes to AutoCenterDbContext
                await _context.SaveChangesAsync();
            }
        }
        
    }
}
