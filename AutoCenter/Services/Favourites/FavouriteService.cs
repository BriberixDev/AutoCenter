using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Results;
using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Services.Favourites
{
    public class FavouriteService : IFavouriteService
    {
        private readonly AutoCenterDbContext _db;

        public FavouriteService(AutoCenterDbContext db)
        {
            _db = db;
        }
        public async Task<Result> AddFavouriteAsync(int listingId, string UserId, CancellationToken ct)
        {
            var exists = await _db.Favourites
                .AnyAsync(f => f.ListingId == listingId && f.OwnerId == UserId, ct);
            if (exists)
            {
                return Result.Fail("Listing is already in favourites");
            }


            var listingExists = await _db.Listings
                .AnyAsync(l => l.Id == listingId, ct);
            if (!listingExists)
            {
                return Result.Fail("Listing does not exist");
            }
            _db.Favourites.Add(new Models.Favourite
            {
                ListingId = listingId,
                OwnerId = UserId,
                AddedOn = DateTime.Now,
                AddedOnUtc = DateTime.UtcNow
            });
            await _db.SaveChangesAsync(ct);
            return Result.Ok();
        }
        public async Task<Result> RemoveFavouriteAsync(int listingId, string UserId, CancellationToken ct)
        {
           var favourites = await _db.Favourites
                .Where(f => f.ListingId == listingId && f.OwnerId == UserId)
                .ToListAsync(ct);
            if (!favourites.Any())
            {
                return Result.Fail("Listing is not in favourites");
            }
            _db.Favourites.RemoveRange(favourites);
            await _db.SaveChangesAsync(ct);
            return Result.Ok();
        }
        public async Task<bool> IsFavouriteAsync(int listingId, string UserId, CancellationToken ct = default)
        {
            return await _db.Favourites
                .AnyAsync(f => f.ListingId == listingId && f.OwnerId == UserId, ct);
        }
        public async Task<IReadOnlyList<Listing>> GetUserFavouritesAsync(string userId, CancellationToken ct = default)
        {
            return await _db.Favourites
                .Where(f => f.OwnerId == userId)

                .Include(f => f.Listing).ThenInclude(l => l.Vehicle).ThenInclude(v => v.Brand)
                .Include(f => f.Listing).ThenInclude(l => l.Vehicle).ThenInclude(v => v.CarModel)
                .Include(f => f.Listing).ThenInclude(l => l.Images)

                .Select(f => f.Listing)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
