using AutoCenter.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Infrastructure.Data.Seed
{
    public class CarBrandSeeder
    {
        private readonly AutoCenterDbContext _context;

        public CarBrandSeeder(AutoCenterDbContext autoCenterDbContext)
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
            var existingNames = await _context.CarBrands
                .Select(m => m.Name)
                .ToListAsync();

            var existing = new HashSet<string>(
                existingNames.Select(NormalizeName),
                StringComparer.OrdinalIgnoreCase
            );

            var newMakes = BrandNames
                .Select(NormalizeName)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Where(n => !existing.Contains(n))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(n => new Brand { Name = n })
                .ToList();

            if (newMakes.Count == 0) return;

            await _context.CarBrands.AddRangeAsync(newMakes);
            await _context.SaveChangesAsync();
        }

        private static string NormalizeName(string name) =>
            string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
    }
}
