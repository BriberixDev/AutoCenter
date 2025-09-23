using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Images;
using AutoCenter.Web.Models;
using AutoCenter.Web.Services.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AutoCenter.Web.Services.Listings
{
    public class ListingImageService : IListingImageService
    {
        private readonly AutoCenterDbContext _db;
        private readonly IImageStorage _storage;
        private readonly ImageStorageOptions _opt;

        public ListingImageService(AutoCenterDbContext db, IImageStorage storage, IOptions<ImageStorageOptions> opt)
        {
            _db = db;
            _storage = storage;
            _opt = opt.Value;
        }

        public async Task<IReadOnlyList<string>> AddImagesAsync(int listingId , IEnumerable<IFormFile> files,CancellationToken ct = default)
        {
            if(files == null) throw new ArgumentNullException(nameof(files));

            var input = files.Where(f => f != null && f.Length >0).ToList();
            if(input.Count ==0) throw new ArgumentException("No files provided", nameof(files));

            var existingCount = await _db.ListingImages.CountAsync(li => li.ListingId == listingId, ct);
            if(existingCount + input.Count > _opt.MaxFilesPerListing)
            {
                throw new InvalidOperationException($"Limit {_opt.MaxFilesPerListing} images per listing");
            }

            foreach(var file in input)
            {
                if (_opt.AllowedContentTypes?.Length > 0 &&
                    !_opt.AllowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Unsupported content type: {file.ContentType}");
                }

                if (file.Length > _opt.MaxFileSizeInBytes)
                    throw new InvalidOperationException("File is too large.");
            }

            var startOrder = await _db.ListingImages
                .Where(x => x.ListingId == listingId)
                .OrderByDescending(x => x.SortOrder)
                .Select(x => (int?)x.SortOrder)
                .FirstOrDefaultAsync(ct) ?? -1;

            var SavedPaths = new List<string>();
            var newImages = new List<ListingImage>();

            try
            {
                foreach (var file in input)
                {
                    var relPath = await _storage.SaveAsync(listingId, file, ct);
                    SavedPaths.Add(relPath);

                    var img = new ListingImage
                    {
                        ListingId = listingId,
                        FileName = Path.GetFileName(relPath),
                        ContentType = file.ContentType,
                        FileSize = file.Length,
                        RelativePath = relPath,
                        SortOrder = ++startOrder,
                    };
                    newImages.Add(img);
                    _db.ListingImages.Add(img);
                }
                var hasPrimary = await _db.ListingImages
                                        .AnyAsync(x => x.ListingId == listingId && x.IsPrimary, ct);
                if (!hasPrimary && newImages.Count > 0)
                    newImages[0].IsPrimary = true;
                await _db.SaveChangesAsync(ct);
                return SavedPaths;
            }
            catch
            {
                foreach(var path in SavedPaths)
                {
                    try
                    {
                        await _storage.DeleteAsync(path, ct);
                    }
                    catch
                    {
                       //soon add logs
                    }
                    
                }
                throw;
            }

            
        }
    }
}
