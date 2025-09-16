using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class AgencyUser:ApplicationUser
    {
        public  string AgencyName{ get; set; } = string.Empty;
        public  string AgencyAddress { get; set; } = string.Empty;
        public  string CompanyId { get; set; } = string.Empty;
        [Url]
        public string WebsiteUrl { get; set; } = string.Empty;
    }
}
