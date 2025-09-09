    namespace AutoCenter.Web.Models
    {
        public class CarModel
        {
            public int Id { get; set; }
            public int BrandId { get; set; }
            public string Name { get; set; } = null!;
            public Brand Brand { get; set; } = null!;
        }
    }
