using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class Listing 
    {
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [StringLength(5000, MinimumLength = 10)]
        public  string Description { get; set; } = string.Empty;

        [Required, Range(1,10_000_000)]
        public  decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public string? OwnerId { get; set; }
        public ApplicationUser Owner { get; set; } = null!;

        public int VehicleSpecId { get; set; }
        public VehicleSpec Vehicle { get; set; } = null!; //FIX 

        public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();

        public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

        public string MainImagePath =>
             Images.FirstOrDefault(i=>i.IsPrimary)?.RelativePath ?? "/images/AutoCenterDefault.jpg";
    }

}
