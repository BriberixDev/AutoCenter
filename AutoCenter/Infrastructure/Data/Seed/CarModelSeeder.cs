using AutoCenter.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Infrastructure.Data.Seed
{
    public class CarModelSeeder
    {
        private readonly AutoCenterDbContext _context;

        public CarModelSeeder(AutoCenterDbContext autoCenterDbContext)
        {
            _context = autoCenterDbContext;
        }

        public static readonly Dictionary<string, List<string>> CarModelsByBrand = new()
        {
            { "Audi", new() { "A3", "A4", "A6", "Q5", "Q7" } },
            { "BMW", new() { "3 Series", "5 Series", "X3", "X5" } },
            { "Mercedes-Benz", new() { "C-Class", "E-Class", "GLC", "GLE" } },
            { "Porsche", new() { "911", "Cayenne", "Macan" } }
        };

        public async Task SeedAsync(CancellationToken ct = default)
        {
            var brandsInDb = await _context.CarBrands
                .Include(b => b.CarModels)
                .ToListAsync(ct);

            var brandMap = brandsInDb.ToDictionary(
                b => NormalizeName(b.Name),
                StringComparer.OrdinalIgnoreCase
            );

            foreach (var (rawBrandName, rawModelNames) in CarModelsByBrand)
            {
                var brandName = NormalizeName(rawBrandName);
                var modelNames = rawModelNames
                    .Select(NormalizeName)
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .ToList();

                if (!brandMap.TryGetValue(brandName, out var brandEntity))
                {
                    brandEntity = new Brand
                    {
                        Name = brandName,
                        CarModels = new List<CarModel>()
                    };
                    _context.CarBrands.Add(brandEntity);
                    brandMap[brandName] = brandEntity;
                }

                var existingModelNames = new HashSet<string>(
                    brandEntity.CarModels.Select(m => NormalizeName(m.Name)),
                    StringComparer.OrdinalIgnoreCase
                );

                foreach (var modelName in modelNames)
                {
                    if (existingModelNames.Contains(modelName)) continue;

                    brandEntity.CarModels.Add(new CarModel
                    {
                        Name = modelName
                    });
                }
            }

            await _context.SaveChangesAsync(ct);
        }

        private static string NormalizeName(string name) =>
            string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
    }
}
