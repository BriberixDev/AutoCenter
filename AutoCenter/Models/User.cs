namespace AutoCenter.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        public required string Email { get; set; }

        // Additional properties can be added as needed
    }
}
