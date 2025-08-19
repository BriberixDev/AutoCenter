namespace AutoCenter.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool Role { get; set; }=false;
        public DateTime RegistrationDate { get; private set; } = DateTime.Now;
    }
}
