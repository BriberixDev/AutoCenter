namespace AutoCenter.Web.Services
{
    public interface IFileService
    {
        Task<string> SaveAsync(IFormFile file, CancellationToken ct = default);
    }
}
