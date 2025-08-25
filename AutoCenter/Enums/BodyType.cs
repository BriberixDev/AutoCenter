using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Enums
{
    public enum BodyType
    {
        [Display(Name = "Sedan")]
        Sedan=1,

        [Display(Name = "Hatchback")]
        Hatchback=2,

        [Display(Name = "Wagon")]
        Wagon=3,

        [Display(Name = "Coupe")]
        Coupe=4,

        [Display(Name = "SUV")]
        SUV=5
    }
}
