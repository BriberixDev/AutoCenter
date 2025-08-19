using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class AgencyUser:User
    {
        public required string AgencyName{ get; set; } = string.Empty;
        public required string AgencyAddress { get; set; } = string.Empty;
        public required string CompanyId { get; set; } = string.Empty;
        [Url]
        public string WebsiteUrl { get; set; } = string.Empty;
    }
}
