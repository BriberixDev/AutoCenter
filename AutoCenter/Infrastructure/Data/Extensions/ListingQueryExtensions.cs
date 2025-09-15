using AutoCenter.Web.Dtos.Search;
using AutoCenter.Web.Models;

namespace AutoCenter.Web.Infrastructure.Data.Extensions
{
    public static class ListingQueryExtensions
    {
        public static IQueryable<Listing> ApplyFilters(this IQueryable<Listing> query, SearchFiltersDto f)
        {
            if (f.BrandId is int bid)
                query = query.Where(l => l.Vehicle.BrandId == bid);
            if (f.ModelId is int mid)
                query = query.Where(l => l.Vehicle.CarModelId == mid);

            if (f.MinPrice is int p1)
                query = query.Where(l => l.Price >= p1);
            if (f.MaxPrice is int p2)
                query = query.Where(l => l.Price <= p2);

            if (f.MinYear is int y1)
                query = query.Where(l => l.Vehicle.Year >= y1);
            if (f.MaxYear is int y2)
                query = query.Where(l => l.Vehicle.Year <= y2);

             if (f.MinMileage is int m1)
                query = query.Where(l => l.Vehicle.Mileage >= m1);
            if (f.MaxMileage is int m2)
                query = query.Where(l => l.Vehicle.Mileage <= m2);

            return query;
        }
    }
}
