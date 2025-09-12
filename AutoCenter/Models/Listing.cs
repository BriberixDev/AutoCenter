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

        //public required DateTime CreatedAt { get; set; } = DateTime.Now;
        //public required DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int VehicleId { get; set; }
        public VehicleSpec Vehicle { get; set; } = null!;
    }

}
