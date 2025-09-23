namespace AutoCenter.Web.Services.Listings
{
    public interface IListingImageService
    {
        Task<IReadOnlyList<string>> AddImagesAsync(int listingId, IEnumerable<IFormFile> files, CancellationToken ct = default);
        //Task RemoveImageAsync(int listingId, string imagePath, CancellationToken ct = default);
    }
}
