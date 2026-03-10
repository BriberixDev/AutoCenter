using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(64)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(64)]
        public string LastName { get; set; } = string.Empty;

        public byte[]? ProfilePicture { get; set; }

        public DateTime RegistrationDate { get; private set; } = DateTime.UtcNow;

        public List<Listing> Listings { get; set; } = new();
        public List<Favourite> Favourites { get; set; } = new();



        // Properties for reviews
        public decimal AverageRating { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;
        public ICollection<Review> WrittenReviews { get; set; } = new List<Review>();
        public ICollection<Review> ReceivedReviews { get; set; } = new List<Review>();
    }
}

