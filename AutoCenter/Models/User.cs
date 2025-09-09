    using System.ComponentModel.DataAnnotations;

    namespace AutoCenter.Web.Models
    {
        public class User
        {
            public int Id { get; set; }

            [Required, MaxLength(40)]
            public required string Name { get; set; }
            [Required,EmailAddress, MaxLength(50)]
            public  required string Email { get; set; }
            public string? PhoneNumber { get; set; }
            public bool Role { get; set; }=false;
            public DateTime RegistrationDate { get; private set; } = DateTime.Now;
        }
    }
