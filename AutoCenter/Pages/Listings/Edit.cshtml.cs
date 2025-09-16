using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Models;
using AutoCenter.Web.Infrastructure.Data;

namespace AutoCenter.Web.Pages.Listings
{
    public class EditModel : PageModel
    {
        private readonly AutoCenterDbContext _context;

        public EditModel(AutoCenterDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Listing Listing { get; set; } = new Listing();
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                _context.Listings.Add(Listing);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to save. Try again.");
                return Page();
            }
        }

    }
}
