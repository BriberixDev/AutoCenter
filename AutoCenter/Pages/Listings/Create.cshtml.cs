using AutoCenter.Web.Enums;
using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Images;
using AutoCenter.Web.Models;
using AutoCenter.Web.Services.Listings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoCenter.Web.Pages.Listings
{
    
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AutoCenterDbContext _context;
        private readonly IListingImageService _imageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CreateModel> _logger;
        private readonly ImageStorageOptions _imgOpt;

        public CreateModel(AutoCenterDbContext context, IListingImageService imageService, UserManager<ApplicationUser> userManager, ILogger<CreateModel> logger, IOptions<ImageStorageOptions> imgOpt)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
            _logger = logger;
            _imgOpt = imgOpt.Value;
        }
        [BindProperty]
        public CreateListingInputModel Input { get; set; } = new ();
       
        [BindProperty] 
        public IList<IFormFile> Photos { get; set; } = new List<IFormFile>();

        public IEnumerable<SelectListItem> BrandOptions { get; private set; } = [];
        public IEnumerable<SelectListItem> ModelOptions { get; private set; } = [];

        public int MaxPhotos => _imgOpt.MaxPhotos;
        public int MaxFileSizeInMb => (int)(_imgOpt.MaxFileSizeInBytes / (1024 * 1024));

        private void PushOptionsToViewData()
        {
            ViewData["BrandOptions"] = BrandOptions;
            ViewData["ModelOptions"] = ModelOptions;
        }
        public async Task LoadBrandsAsync(CancellationToken ct)
        {
            BrandOptions = await _context.CarBrands
                .AsNoTracking()
                .OrderBy(b => b.Name)
                .Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                })
                .ToListAsync(ct);
        }
        public async Task LoadModelsAsync(int brandId, CancellationToken ct)
        {
            ModelOptions = await _context.CarModels
                .AsNoTracking()
                .Where(m => m.BrandId == brandId)
                .OrderBy(m => m.Name)
                .Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                })
                .ToListAsync(ct);
        }

        public async Task<IActionResult> OnGetModelsAsync(int brandId, CancellationToken ct)
        {
            await LoadModelsAsync(brandId, ct);
            return new JsonResult(ModelOptions);
        }

        public async Task<JsonResult> OnGetModelAsync(int brandId, CancellationToken ct)
        {
            var models = await _context.CarModels
                .AsNoTracking()
                .Where(m => m.BrandId == brandId)
                .OrderBy(m => m.Name)
                .Select(m => new { id = m.Id, name = m.Name })
                .ToListAsync(ct);

            return new JsonResult(models);
        }
        public async Task OnGet(CancellationToken ct)
        {
            await LoadBrandsAsync(ct);
            if (Input.BrandId > 0)
                await LoadModelsAsync(Input.BrandId, ct);

            PushOptionsToViewData();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await LoadBrandsAsync(ct);
                if (Input.BrandId > 0)
                    await LoadModelsAsync(Input.BrandId, ct);
                PushOptionsToViewData();
                return Page();
            }
            var userId = _userManager.GetUserId(User)!;

            await using var tx = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var spec = new VehicleSpec()
                {
                    BrandId= Input.BrandId,
                    CarModelId= Input.ModelId,
                    Year= Input.Year,
                    Mileage= Input.Mileage,
                    Transmission= Input.Transmission,
                    FuelType= Input.FuelType,
                    BodyType= Input.BodyType
                };
                _context.VehicleSpecs.Add(spec);
                
                await _context.SaveChangesAsync(ct);


                var listing = new Listing
                {
                    Title = Input.Title,
                    Description = Input.Description,
                    Price = Input.Price,
                    OwnerId = userId,
                    VehicleSpecId = spec.Id,
                    IsActive = true,
                };
                _context.Listings.Add(listing);
                await _context.SaveChangesAsync(ct);

                if(Photos is { Count: > 0 })
                {
                    await _imageService.AddImagesAsync(listing.Id, Photos, ct);
                }

                await tx.CommitAsync(ct);
                _logger.LogInformation("New listing {ListingId} created by user {UserId}", listing.Id, userId);

                return RedirectToPage("./Details", new { id = listing.Id });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                _logger.LogError(ex, "Error creating listing for user {UserId}", userId);
                ModelState.AddModelError(string.Empty, "Failed to create listing. Please try again.");
                await LoadBrandsAsync(ct);
                if (Input.BrandId > 0)
                    await LoadModelsAsync(Input.BrandId, ct);
                PushOptionsToViewData();
                return Page();
            }
  
        }

    }
}
