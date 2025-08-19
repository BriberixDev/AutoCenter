using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Enums
{
    public enum TransmissionType
    {
        [Display(Name = "Manual")]
        Manual = 1,
        [Display(Name = "Automatic")]
        Automatic = 2,
        [Display(Name = "Semi-Automatic")]
        SemiAutomatic = 3,
        [Display(Name = "Continuously Variable Transmission (CVT)")]
        CVT = 4,
        [Display(Name = "DSG/DCT")]
        DualClutch = 5
    }
}
