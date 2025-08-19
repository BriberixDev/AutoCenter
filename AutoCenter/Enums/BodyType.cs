using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Enums
{
    public enum BodyType
    {
        [Display(Name = "Sedan")]
        Sedan,

        [Display(Name = "Hatchback")]
        Hatchback,

        [Display(Name = "Wagon")]
        Wagon,

        [Display(Name = "Coupe")]
        Coupe,

        [Display(Name = "SUV")]
        SUV
    }
}
