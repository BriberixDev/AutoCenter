using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Enums
{
    public enum FuelType
    {
        [Display(Name = "Petrol")]
        Petrol=1,
        [Display(Name = "Diesel")]
        Diesel=2,
        [Display(Name = "Electric")]
        Electric=3,
        [Display(Name = "Hybrid")]
        Hybrid=4,
        [Display(Name = "Gas")]
        Gas=5
    }
}
