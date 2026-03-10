namespace AutoCenter.Web.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
        public string TargetUserId { get; set; } = null!;
        public ApplicationUser TargetUser { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted{ get; set; }
    }
}
