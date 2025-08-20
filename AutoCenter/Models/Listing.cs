namespace AutoCenter.Web.Models
{
    public class Listing 
    {
        public int Id { get; set; }
        public  string Title { get; set; } = string.Empty;
        public  string Description { get; set; } = string.Empty;
        public  decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        //public required DateTime CreatedAt { get; set; } = DateTime.Now;
        //public required DateTime UpdatedAt { get; set; } = DateTime.Now;
        public  VehicleSpecsSpecs VehicleSpecs { get; set; }=new();

    }

}
