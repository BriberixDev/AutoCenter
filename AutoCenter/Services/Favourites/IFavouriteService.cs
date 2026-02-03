using AutoCenter.Web.Models;
using AutoCenter.Web.Infrastructure.Results;

namespace AutoCenter.Web.Services.Favourites
{
    public interface IFavouriteService 
    {
        Task<Result> AddFavouriteAsync(int listingId, string userId, CancellationToken ct);
        Task<Result> RemoveFavouriteAsync(int listingId, string userId, CancellationToken ct);
        Task<bool> IsFavouriteAsync(int listingId, string userId, CancellationToken ct = default);

        Task<IReadOnlyList<Listing>> GetUserFavouritesAsync(string userId, CancellationToken ct = default);
    }
}
