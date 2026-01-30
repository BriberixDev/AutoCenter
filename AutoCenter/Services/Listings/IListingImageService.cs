namespace AutoCenter.Web.Services.Listings
{
    public interface IListingImageService
    {
        Task<IReadOnlyList<string>> AddImagesAsync(int listingId, IEnumerable<IFormFile> files, CancellationToken ct = default);
        Task RemoveImagesAsync(int listingId, IEnumerable<int> imageIds, CancellationToken ct = default);
    }
}
