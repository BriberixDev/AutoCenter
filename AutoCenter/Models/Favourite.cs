namespace AutoCenter.Web.Models
{
    public class Favourite
    {
        public string OwnerId { get; set; } = null!;
        public ApplicationUser Owner { get; private set; } = null!;

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;

        public DateTime AddedOnUtc { get; init; } = DateTime.UtcNow;
    }
}
