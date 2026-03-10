namespace AutoCenter.Web.Dtos.Review
{
    public sealed class ReviewViewDto
    {
        public int Rating { get; set; } 
        public string? Comment { get; set; }
        public string AuthorId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
