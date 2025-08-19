using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Enums
{
    public enum FuelType
    {
        [Display(Name = "Petrol")]
        Petrol,
        [Display(Name = "Diesel")]
        Diesel,
        [Display(Name = "Electric")]
        Electric,
        [Display(Name = "Hybrid")]
        Hybrid,
        [Display(Name = "Gas")]
        Gas
    }
}
