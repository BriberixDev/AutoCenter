using AutoCenter.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Pages.Listings
{
    public class CreateListingInputModel
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = "";

        [StringLength(5000, MinimumLength = 10)]
        public string? Description { get; set; }

        [Range(1, 10_000_000)]
        public decimal Price { get; set; }

        [Required] public int BrandId { get; set; }
        [Required] public int ModelId { get; set; }

        [Range(1950, 2026)]
        public int Year { get; set; }

        [Range(0, 999)]
        public int Mileage { get; set; }

        [Required] public TransmissionType Transmission { get; set; }
        [Required] public FuelType FuelType { get; set; }
        [Required] public BodyType BodyType { get; set; }
        //public isActive {get;set } = true;
    }
}
