using AutoCenter.Web.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public AutoCenter.Web.Models.Listing Listing { get; set; } = new AutoCenter.Web.Models.Listing();
        public void OnGet()
        {

        }
    }
}
