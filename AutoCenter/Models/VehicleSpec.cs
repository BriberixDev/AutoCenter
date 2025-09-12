using AutoCenter.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class VehicleSpec
    {
        public int Id { get; set; }
        [Required]public int BrandId { get; set; }
        public Brand? Brand { get; set; } = null!;
        [Required]public int CarModelId { get; set; }
        public CarModel? CarModel { get; set; }
        [Required,Range(1930,2025)]public int Year { get; set; } 
        [Required, Range(0, 999)] public int Mileage { get; set; } 

        //[Required]public VehicleSpecColor? Color { get; set; }
        [Required,StringLength(32)]public string Vin { get; set; } = string.Empty;
        [Required]public TransmissionType? Transmission { get; set; }
        [Required]public FuelType? FuelType { get; set; }
        [Required]public BodyType? BodyType { get; set; }


    }
}
