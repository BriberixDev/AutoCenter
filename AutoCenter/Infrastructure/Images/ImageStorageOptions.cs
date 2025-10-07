namespace AutoCenter.Web.Infrastructure.Images
{
    public sealed class ImageStorageOptions
    {
        public int MaxPhotos { get; set; } = 12;

        public const int DefaultMaxFileSizeInMb = 10;
        public long MaxFileSizeInBytes { get; set; } = DefaultMaxFileSizeInMb * 1024 * 1024; // Max weight for image 10MB

        public string[] AllowedContentTypes { get; init; } = new[]
        {
            "image/jpg",
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        public string RootRelativePath { get; set; } = "/uploads/listings";
    }
}
