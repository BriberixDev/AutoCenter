using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Display(Name = "Brand")]
        public string Name { get; set; }= null!;

        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();//Conection with CarModel (one to many)
    }
}
