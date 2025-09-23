using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        [Display(Name = "Model")]
        public string Name { get; set; } = null!;


        public Brand Brand { get; set; } = null!;
        public VehicleSpec Spec { get; set; } = null!;
    }
}

