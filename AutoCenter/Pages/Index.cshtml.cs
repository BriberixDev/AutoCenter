using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Listing> Listings { get; set; } = new();

        public async Task OnGetAsync()
        {
            Listings = await _context.Listings
                .Include(l => l.VehicleSpecs)
                .ToListAsync();
        }
    }
}
