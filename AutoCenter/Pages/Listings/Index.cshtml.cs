using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Models;
using AutoCenter.Web.Infrastructure.Data;

namespace AutoCenter.Web.Pages.Listings
{
    public class IndexModel : PageModel
    {
        private readonly AutoCenterDbContext _context;

        public IndexModel(AutoCenterDbContext context)
        {
            _context = context;
        }

        public IList<Listing> Listing { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Listing = await _context.Listings.ToListAsync();
        }
    }
}
