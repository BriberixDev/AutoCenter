namespace AutoCenter.Web.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }= null!;
        public ICollection<VehicleSpecs> Vehicles { get; set; } = new List<VehicleSpecs>();

        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>(); //Conection with CarModel (one to many)
    }
}
