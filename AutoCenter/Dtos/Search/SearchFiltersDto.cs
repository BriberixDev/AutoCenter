namespace AutoCenter.Web.Dtos.Search
{
    public class SearchFiltersDto
    {
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinMileage { get; set; }
        public int? MaxMileage { get; set; }

        public void Normalize()
        {
            if (MinYear is > 0 && MaxYear is > 0 && MinYear > MaxYear)
                (MinYear, MaxYear) = (MaxYear, MinYear);

            if (MinPrice is > 0 && MaxPrice is > 0 && MinPrice > MaxPrice)
                (MinPrice, MaxPrice) = (MaxPrice, MinPrice);

            if (MinMileage is > 0 && MaxMileage is > 0 && MinMileage > MaxMileage)
                (MinMileage, MaxMileage) = (MaxMileage, MinMileage);
        }
    }
}
