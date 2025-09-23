namespace AutoCenter.Web.Infrastructure.Images
{
    public sealed class ImageStorageOptions
    {
        public int MaxFilesPerListing { get; set; } = 10;
        public long MaxFileSizeInBytes { get; set; } = 10 * 1024 * 1024; // Max weight for image 10MB

        public string[] AllowedContentTypes { get; init; } = new[]
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        public string RootRelativePath { get; set; } = "/uploads/listings";
    }
}
