using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AutoCenter.Web.Pages.Listings
{
    public class DeleteModel : PageModel
    {
        private readonly AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext _context;

        public DeleteModel(AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Listing Listing { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Listing = await _context.Listings
    .Include(l => l.VehicleSpecs)
    .FirstOrDefaultAsync(m => m.Id == id);
            if (Listing == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            Listing = await _context.Listings.FindAsync(id);
            if (Listing != null)
            {
                _context.Listings.Remove(Listing);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }


    }
}
