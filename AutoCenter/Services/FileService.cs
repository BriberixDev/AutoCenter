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
        public async Task<string> SaveAsync(IFormFile file, CancellationToken ct = default)
        {
            if(file==null || file.Length == 0)
                throw new ArgumentException("File is null or empty", nameof(file));

            var AllowedExtensions = new [] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type.", nameof(file));
            const long maxBytes = 5 * 1024 * 1024;
            if (file.Length > maxBytes)
                throw new InvalidOperationException("This file is too big.");
            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid().ToString("N") + extension;
            var filePath = Path.Combine(uploads, fileName);
            await using (var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(stream, ct);
            }
            return "/uploads/" + fileName;
        }  
    }
}
