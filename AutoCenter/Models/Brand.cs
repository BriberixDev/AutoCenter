namespace AutoCenter.Web.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<VehicleSpecs> Vehicles { get; set; } = new List<VehicleSpecs>();
    }
}
