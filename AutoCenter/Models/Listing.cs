namespace AutoCenter.Web.Models
{
    public class Listing
    {
        public int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required decimal Price { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;
        public required DateTime UpdatedAt { get; set; } = DateTime.Now;
        public required User Owner { get; set; }
        public required Vehicle Vehicle { get; set; }
        public bool IsActive { get; set; } = true;
        // Additional properties can be added as needed
    }
}
