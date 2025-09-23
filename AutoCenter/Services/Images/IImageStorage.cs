namespace AutoCenter.Web.Services.Images
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(int listingId, IFormFile file, CancellationToken ct = default);
        Task DeleteAsync(string imagePath, CancellationToken ct = default);
    }
}
