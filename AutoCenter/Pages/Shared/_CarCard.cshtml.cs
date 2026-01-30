using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Models;
using AutoCenter.Web.Services.Favourites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Pages.Shared
{
    public class _CarCardModel
    {
        private readonly AutoCenterDbContext _context;
        
        public _CarCardModel(AutoCenterDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public IList<Listing> Listing { get; private set; } = [] ;
        public async Task OnGetAsync(int? id)
        {
            Listing = await _context.Listings
         .Include(x => x.Vehicle).ThenInclude(v => v.Brand)
         .Include(x => x.Vehicle).ThenInclude(v => v.CarModel)
         .Include(x => x.Images)
         .AsNoTracking()
         .ToListAsync();


        }
        
    }
}
