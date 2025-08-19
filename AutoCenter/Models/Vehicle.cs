namespace AutoCenter.Web.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public required string Make { get; set; } = string.Empty;
        public required string Model { get; set; } = string.Empty;
        public required string Year { get; set; } = string.Empty;
        public required string Color { get; set; } = string.Empty;
        public required string Milieage { get; set; } = string.Empty;
        public required string Vin { get; set; } = string.Empty;
        public required string Transmission { get; set; } = string.Empty;
        public required string FuelType { get; set; } = string.Empty;
        public required string BodyType { get; set; } = string.Empty;


    }
}
