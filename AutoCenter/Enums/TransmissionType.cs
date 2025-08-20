using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Enums
{
    public enum TransmissionType
    {
        [Display(Name = "Manual")]
        Manual,
        [Display(Name = "Automatic")]
        Automatic,
        [Display(Name = "Semi-Automatic")]
        SemiAutomatic,
        [Display(Name = "Continuously Variable Transmission (CVT)")]
        CVT,
        [Display(Name = "DSG/DCT")]
        DualClutch
    }
}
