namespace AutoCenter.Web.Models
{
    public class Listing :Vehicle
    {
        public int Id { get; set; }
        public  string Title { get; set; } = string.Empty;
        public  string Description { get; set; } = string.Empty;
        public  decimal Price { get; set; }

        //public required DateTime CreatedAt { get; set; } = DateTime.Now;
        //public required DateTime UpdatedAt { get; set; } = DateTime.Now;
        public  User Owner { get; set; }
        public  Vehicle Vehicle { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
