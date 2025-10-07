using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCenter.Web.Pages.Listings
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly AutoCenterDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(AutoCenterDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
    .Include(l => l.Vehicle)
    .FirstOrDefaultAsync(m => m.Id == id);
            if (Listing == null)
            {
                return NotFound();
            }
            
            var currentUserId = _userManager.GetUserId(User);
            if (Listing.OwnerId != currentUserId)
            {
                return Forbid();
            }
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var listing = await _context.Listings.FindAsync(id);
            if(listing==null)
            {
                return NotFound();
            }
            var currentUserId = _userManager.GetUserId(User);
            if (listing.OwnerId != currentUserId)
                return Forbid();

            _context.Listings.Remove(Listing);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }


    }
}
