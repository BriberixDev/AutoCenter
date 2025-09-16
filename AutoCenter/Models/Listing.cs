using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class Listing 
    {
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, MinimumLength = 10)]
        public  string Description { get; set; } = string.Empty;

        [Required, Range(1,10_000_000)]
        public  decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public string? OwnerId { get; set; }
        public ApplicationUser Owner { get; set; } = null!;

        public int VehicleSpecId { get; set; }
        public VehicleSpec Vehicle { get; set; } = null!;
    }

}
