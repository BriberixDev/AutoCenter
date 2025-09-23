namespace AutoCenter.Web.Models
{
    public class ListingImage
    {
        public int Id { get; set; }

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;

        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long FileSize { get; set; }
        public string RelativePath { get; set; } = null!;

        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
