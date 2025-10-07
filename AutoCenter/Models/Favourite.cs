namespace AutoCenter.Web.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = null!;
        public ApplicationUser Owner { get; private set; } = null!;

        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;

        public DateTime AddedOn { get; private set; } = DateTime.UtcNow;
        public DateTime AddedOnUtc { get; private set; } = DateTime.UtcNow;
    }
}
