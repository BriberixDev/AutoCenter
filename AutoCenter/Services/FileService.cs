using static System.Net.Mime.MediaTypeNames;

namespace AutoCenter.Web.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env; //Gives information about project structure
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string> SaveAsync(IFormFile file, string? subfolder = null, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0) throw new ArgumentException("File is null or empty", nameof(file));

            var allowed = new[] { ".jpg",".jpeg", ".png",".webp"};
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext)) throw new ArgumentException("Invalid file type.", nameof(file));

            const long maxBytes = 5 * 1024 * 1024;
            if (file.Length > maxBytes) throw new InvalidOperationException("This file is too big");

            var root = Path.Combine(_env.WebRootPath, "uploads", subfolder ?? "", DateTime.UtcNow.ToString("yyyy/MM"));
            Directory.CreateDirectory(root);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(root, fileName);

            await using var stream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(stream, ct);

            var rel = Path.GetRelativePath(_env.WebRootPath, fullPath).Replace('\\', '/');
            return "/" + rel;
        }
    }
}
