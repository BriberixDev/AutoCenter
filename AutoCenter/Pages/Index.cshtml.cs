using AutoCenter.Web.Dtos.Search;
using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Data.Extensions;
using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace AutoCenter.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AutoCenterDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, AutoCenterDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public SearchFiltersDto Filters { get; set; } = new();
        public List<Listing> Listings { get;private set; } = new();

        public async Task OnGetAsync()
        {
            Filters.Normalize();
            
            Filters.Brands = await _context.CarBrands
                .OrderBy(b => b.Name)
                .Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                })
                .ToListAsync();

            if (Filters.BrandId.HasValue)
            {
                Filters.Models = await _context.CarModels
                    .Where(m=>m.BrandId == Filters.BrandId)
                    .OrderBy(m => m.Name)
                    .Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    })
                    .ToListAsync();
            }

            var query = _context.Listings
                .AsNoTracking()
                .Include(l => l.Vehicle).ThenInclude(vs => vs.Brand)
                .Include(l => l.Vehicle).ThenInclude(vs => vs.CarModel)
                .Where(l => l.IsActive)
                .AsQueryable();
            query = query.ApplyFilters(Filters);

            var total = await query.CountAsync();
            Listings = await query
                .OrderByDescending(l => l.Id)
                .Take(24)
                .ToListAsync();
        }
        public async Task<IActionResult> OnGetModelsAsync(int brandId)
        {
            var models = await _context.CarModels
                .Where(m => m.BrandId == brandId)
                .OrderBy(m => m.Name)
                .Select(m => new { m.Id, m.Name })
                .ToListAsync();
            return new JsonResult(models); 
        }
    }
}
