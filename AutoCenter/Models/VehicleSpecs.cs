using AutoCenter.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class VehicleSpecs
    {
        [Required]public int BrandId { get; set; }
        public Brand Brand { get; set; }=null!;
        [Required]public string Model { get; set; } = string.Empty;
        [Required,Range(1930,2025)]public string Year { get; set; } = string.Empty;
        [Required, Range(0, 999)] public string Mileage { get; set; } = string.Empty;

        //[Required]public VehicleSpecsColor? Color { get; set; }
        [Required,StringLength(32)]public string Vin { get; set; } = string.Empty;
        [Required]public TransmissionType? Transmission { get; set; }
        [Required]public FuelType? FuelType { get; set; }
        [Required]public BodyType? BodyType { get; set; }


    }
}
