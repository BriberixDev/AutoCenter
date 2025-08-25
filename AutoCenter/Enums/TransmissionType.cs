using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Enums
{
    public enum TransmissionType
    {
        [Display(Name = "Manual transmission")]
        Manual=1,
        [Display(Name = "Automatic transmission")]
        Automatic=2,
        [Display(Name = "Semi-Automatic transmission")]
        SemiAutomatic=3,
        [Display(Name = "Continuously Variable Transmission (CVT)")]
        CVT=4,
        [Display(Name = "DSG/DCT")]
        DualClutch=5
    }
}
