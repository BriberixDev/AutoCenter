using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            { "Audi", new () { "A3", "A4", "A6", "Q5", "Q7" } },
            { "BMW", new () { "3 Series", "5 Series", "X3", "X5" } },
            { "Mercedes-Benz", new ()  { "C-Class", "E-Class", "GLC", "GLE" } },
            { "Porsche", new ()  { "911", "Cayenne", "Macan" } }
        };
        public async Task SeedAsync()
        {
            var brandsInDb = await _context.Brands.Include(b => b.CarModels).ToListAsync();
            var brandMap = brandsInDb.ToDictionary(b => b.Name, StringComparer.OrdinalIgnoreCase);
            foreach (var obj in CarModelsByBrand)
            {
                var brandName = obj.Key;
                var modelNames  = obj.Value;

                if(!brandMap.TryGetValue(brandName, out var brandEntity))
                {
                    brandEntity = new Brand
                    {
                        Name = brandName,
                        CarModels = new List<CarModel>()
                    };
                    _context.Brands.Add(brandEntity);
                    brandMap[brandName] = brandEntity;
                }   
                

                var existingModelNames = new HashSet<string>(brandEntity.CarModels.Select(m=>m.Name),StringComparer.OrdinalIgnoreCase);

                foreach (var modelName in modelNames)
                {
                    if (existingModelNames.Contains(modelName))continue;

                    brandEntity.CarModels.Add(new CarModel { Name=modelName, Brand=brandEntity});

                }
            }
            await _context.SaveChangesAsync();

        }
    }
}
