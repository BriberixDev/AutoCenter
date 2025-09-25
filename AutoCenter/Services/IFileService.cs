namespace AutoCenter.Web.Services
{
    public interface IFileService
    {
        Task<string> SaveAsync(IFormFile file, string? subfolder = null, CancellationToken ct = default);
    }
}
