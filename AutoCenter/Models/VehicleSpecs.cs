using AutoCenter.Web.Enums;

namespace AutoCenter.Web.Models
{
    public class VehicleSpecsSpecs
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public VehicleSpecsColor? Color { get; set; }
        public string Mileage { get; set; } = string.Empty;
        public string Vin { get; set; } = string.Empty;
        public TransmissionType? Transmission { get; set; }
         public FuelType? FuelType { get; set; }
         public BodyType? BodyType { get; set; }


    }
}
