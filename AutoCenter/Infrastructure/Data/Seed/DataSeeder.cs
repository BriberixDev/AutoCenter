using AutoCenter.Web.Enums;
using AutoCenter.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Infrastructure.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AutoCenterDbContext db, ILogger logger, bool seedDemoListings)
        {
            await db.Database.MigrateAsync();

            await using var tx = await db.Database.BeginTransactionAsync();

            var targetBrands = new[]
            {
                "Audi", "BMW", "Mercedes-Benz", "Skoda", "Volkswagen", "Porsche"
            };

            var existingBrandNames = new HashSet<string>(
                await db.CarBrands.Select(b => b.Name).ToListAsync(),
                StringComparer.OrdinalIgnoreCase);

            var newBrands = targetBrands
                .Where(n => !existingBrandNames.Contains(n))
                .Select(n => new Brand { Name = n })
                .ToList();

            if (newBrands.Count > 0)
            {
                db.CarBrands.AddRange(newBrands);
                await db.SaveChangesAsync();
            }

            var brandMap = await db.CarBrands.ToDictionaryAsync(b => b.Name, b => b.Id,
                StringComparer.OrdinalIgnoreCase);

            var presets = new (string Brand, string[] Models)[]
            {
                ("Audi", new[] { "A3", "A4", "A6", "Q5", "Q7" }),
                ("BMW", new[] { "3 Series", "5 Series", "7 Series", "X3", "X5" }),
                ("Mercedes-Benz", new[] { "C-Class", "E-Class", "S-Class", "GLC", "GLE" }),
                ("Skoda", new[] { "Fabia", "Octavia", "Superb", "Karoq", "Kodiaq" }),
                ("Volkswagen", new[] { "Golf", "Passat", "Tiguan", "Polo", "Touareg" }),
                ("Porsche", new[] { "Cayenne", "Macan", "Panamera", "911", "Taycan" }),
            };

            var existingModelKeys = new HashSet<(int BrandId, string Name)>(
                await db.CarModels.Select(m => new ValueTuple<int, string>(m.BrandId, m.Name)).ToListAsync());

            var modelsToAdd = new List<CarModel>();
            foreach (var (brandName, list) in presets)
            {
                var brandId = brandMap[brandName];
                foreach (var m in list)
                {
                    var key = (brandId, m);
                    if (!existingModelKeys.Contains(key))
                        modelsToAdd.Add(new CarModel { Name = m, BrandId = brandId });
                }
            }
            if (modelsToAdd.Count > 0)
            {
                db.CarModels.AddRange(modelsToAdd);
                await db.SaveChangesAsync();
            }

            if (seedDemoListings && !await db.Listings.AnyAsync())
            {
                var rnd = new Random(50);
                var modelsByBrand = await db.CarModels
                    .GroupBy(m => m.BrandId)
                    .ToDictionaryAsync(g => g.Key, g => g.ToList());

                var brandsById = await db.CarBrands.ToDictionaryAsync(b => b.Id);

                var colors = Enum.GetValues<VehicleSpecColor>();
                var bodies = Enum.GetValues<BodyType>();

                var vinSet = new HashSet<string>(StringComparer.Ordinal);
                string NextVin()
                {
                    const string chars = "ABCDEFGHJKLMNPRSTUVWXYZ0123456789";
                    Span<char> buf = stackalloc char[17];
                    for (int i = 0; i < buf.Length; i++)
                        buf[i] = chars[rnd.Next(chars.Length)];
                    var vin = new string(buf);
                    if (!vinSet.Add(vin)) return NextVin();
                    return vin;
                }

                var listings = new List<Listing>(144);
                for (int i = 0; i < 144; i++)
                {
                    var brandId = brandsById.Keys.ElementAt(rnd.Next(brandsById.Count));
                    var brandName = brandsById[brandId].Name;

                    var listModels = modelsByBrand[brandId];
                    var cm = listModels[rnd.Next(listModels.Count)];

                    var year = rnd.Next(2005, 2025);
                    var mileage = rnd.Next(2, 400);    
                    var price = rnd.Next(3_000, 180_000);

                    var fuel = rnd.Next(0, 3) switch
                    {
                        0 => FuelType.Petrol,
                        1 => FuelType.Diesel,
                        _ => FuelType.Electric
                    };
                    var transmission = rnd.Next(0, 2) == 0
                        ? TransmissionType.Manual
                        : TransmissionType.Automatic;

                    var color = colors.GetValue(rnd.Next(colors.Length)) is VehicleSpecColor c ? c : VehicleSpecColor.Black;
                    var body = bodies.GetValue(rnd.Next(bodies.Length)) is BodyType b ? b : BodyType.Sedan;

                    listings.Add(new Listing
                    {
                        Title = $"{brandName} {cm.Name} - {year}",
                        Description = "Demo listing (seed).",
                        Price = price,
                        IsActive = true,
                        Vehicle = new VehicleSpec
                        {
                            BrandId = brandId,
                            CarModelId = cm.Id,
                            Year = year,
                            Mileage = mileage,
                            FuelType = fuel,
                            Transmission = transmission,
                            Color = color,
                            BodyType = body,
                            Vin = NextVin()
                        }
                    });
                }

                db.Listings.AddRange(listings);
                await db.SaveChangesAsync();
            }

            await tx.CommitAsync();
            logger.LogInformation("Seeding done.");
        }
    }
}
