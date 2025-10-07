using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Models;
using AutoCenter.Web.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace AutoCenter.Web.Pages.Listings
{
    public class DetailsModel : PageModel
    {
        private readonly AutoCenterDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(AutoCenterDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Listing Listing { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Listing = await _context.Listings
                .Include(x=>x.Vehicle)
                .Include(x=>x.Images)
                .SingleOrDefaultAsync(x=> x.Id ==id);

            if (Listing == null)
            {
                return NotFound();
            }

            return Page();
        }

        
    }
}
