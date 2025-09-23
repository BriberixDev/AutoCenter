using AutoCenter.Web.Infrastructure.Images;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace AutoCenter.Web.Services.Images
{
    public class LocalImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly ImageStorageOptions _opt;

        public LocalImageStorage(IWebHostEnvironment env, IOptions<ImageStorageOptions> opt)
        {
            _env = env;
            _opt = opt.Value;
        }
        public async Task<string> SaveAsync(int listingId, IFormFile file, CancellationToken ct = default)
        {
            if(file is null) throw new ArgumentNullException(nameof(file));
            if(file.Length <= 0) throw new InvalidOperationException("File is empty");
            if (file.Length > _opt.MaxFileSizeInBytes)
                throw new InvalidOperationException($"File size {file.Length} exceeds max allowed {_opt.MaxFileSizeInBytes}");
            if(_opt.AllowedContentTypes?.Length >0 && !_opt.AllowedContentTypes.Contains(file.ContentType,StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException($"File type {file.ContentType} is not allowed");


            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = GuessExtension(file.ContentType) ?? ".bin";
            }
            extension = extension.ToLowerInvariant();

            var safeName = $"{Guid.NewGuid():N}{extension}";

            var webroot = _env.WebRootPath ?? throw new InvalidOperationException("Web root path not available.");
            var relDir = $"{_opt.RootRelativePath.TrimEnd('/')}/{listingId}";
            var absDir = CombineWebroot(webroot, relDir);

            Directory.CreateDirectory(absDir);

            var absPath = Path.Combine(absDir, safeName);
            await using (var target = new FileStream(absPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(target, ct);
            }

            var relPath = NormalizationRelative($"{relDir}/{safeName}");
                return relPath;
        }
        public Task DeleteAsync(string imagePath, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(imagePath)) return Task.CompletedTask;

            var webroot = _env.WebRootPath ?? throw new InvalidOperationException("WebRoot not set");
            var absPath = Path.Combine(webroot, imagePath);

            if (File.Exists(absPath))
                File.Delete(absPath);

            return Task.CompletedTask;
        }

        private static string? GuessExtension(string contentType) => contentType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => null
        };

        private static string NormalizationRelative(string path)
        {
            var p = path.Replace('\\', '/');
            if (!p.StartsWith('/')) p = "/" + p;
            return p;
        }
        private static string CombineWebroot(string webroot, string relativePath)
        {
            var relPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(webroot, relPath);
        }
    }
}
