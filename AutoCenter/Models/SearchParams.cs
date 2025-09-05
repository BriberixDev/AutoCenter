using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Models
{
    public class SearchParams
    {
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }

        [Range(0, 10_000_000, ErrorMessage = "PriceFrom must be between 0 and 10,000,000")]
        public decimal? PriceFrom { get; set; }
        [Range(0, 10_000_000, ErrorMessage = "PriceTo must be between 0 and 10,000,000")]
        public decimal? PriceTo { get; set; }

        [Range(1900, 2025, ErrorMessage = "First registration must be between 1900 and 2025")]
        public int? YearFrom { get; set; }
        [Range(1900, 2025, ErrorMessage = "First registration must be between 1900 and 2025")]
        public int? YearTo { get; set; }


    }
}
