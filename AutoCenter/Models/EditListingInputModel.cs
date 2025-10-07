using AutoCenter.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class EditListingInputModel
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(5000, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10_000_000)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int? Mileage { get; set; }

        [Required]
        public TransmissionType? Transmission { get; set; }

        [Required]
        public FuelType? FuelType { get; set; }

        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int Year { get; set; }
    }
}
