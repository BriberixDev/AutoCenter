using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Models;

namespace AutoCenter.Web.Pages.Listings
{
    public class CreateModel : PageModel
    {
        private readonly AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext _context;

        public CreateModel(AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Listing Listing { get; set; } = new() { Vehicle = new Vehicle() };

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            _context.Listings.Add(Listing);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
