using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Images;
using AutoCenter.Web.Models;
using AutoCenter.Web.Services.Listings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCenter.Web.Pages.Listings
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AutoCenterDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IListingImageService _imageService;
        private readonly ImageStorageOptions _imgOpt;

        public EditModel(AutoCenterDbContext context, UserManager<ApplicationUser> userManager, IOptionsSnapshot<ImageStorageOptions> imgOpt, IListingImageService imageService)
        {
            _context = context;
            _userManager = userManager;
            _imgOpt = imgOpt.Value;
            _imageService = imageService;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public EditListingInputModel Input { get; set; } = new EditListingInputModel();
        public Listing Listing { get; private set; }

        [BindProperty]
        public IList<IFormFile> Photos { get; set; } = new List<IFormFile>();
        public IReadOnlyList<ListingImage> ExistingImages { get; private set; } = Array.Empty<ListingImage>();
        public List<int> DeleteImageIds { get; set; } = new List<int>();
        public int MaxPhotos => _imgOpt.MaxPhotos;
        public int MaxFileSizeInMb => (int)(_imgOpt.MaxFileSizeInBytes / (1024 * 1024));
        private async Task LoadListingAsync(CancellationToken ct = default)
        {
            Listing = await _context.Listings
                .Include(l => l.Vehicle).ThenInclude(v => v.Brand)
                .Include(l => l.Vehicle).ThenInclude(v => v.CarModel)
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == Id, ct);
        }
        public async Task<IActionResult> OnGetAsync(CancellationToken ct)
        {
            await LoadListingAsync(ct);
            if (Listing == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (Listing.OwnerId != currentUserId) return Forbid();

            Input = new EditListingInputModel
            {
                Title = Listing.Title,
                Description = Listing.Description,
                Price = Listing.Price,
                Mileage = Listing.Vehicle?.Mileage,
                Transmission = Listing.Vehicle?.Transmission,
                FuelType = Listing.Vehicle?.FuelType,
                BrandId = Listing.Vehicle?.BrandId ?? 0,
                ModelId = Listing.Vehicle?.CarModelId ?? 0,
                Year = Listing.Vehicle?.Year ?? 0
            };

            ExistingImages = Listing.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            var listing = await _context.Listings
        .Include(l => l.Vehicle)
        .Include(l => l.Images)
        .FirstOrDefaultAsync(l => l.Id == Id, ct);

            if (listing == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (listing.OwnerId != currentUserId) return Forbid();

            if (!ModelState.IsValid)
            {
                await LoadListingAsync(ct);
                ExistingImages = listing.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).ToList();
                return Page();
            }

            listing.Title = Input.Title;
            listing.Description = Input.Description;
            listing.Price = Input.Price;

            if (listing.Vehicle == null)
            {
                listing.Vehicle = new VehicleSpec
                {
                    BrandId = Input.BrandId,
                    CarModelId = Input.ModelId,
                    Year = Input.Year
                };
            }
            listing.Vehicle.Mileage = Input.Mileage ?? listing.Vehicle.Mileage;
            listing.Vehicle.Transmission = Input.Transmission!.Value;
            listing.Vehicle.FuelType = Input.FuelType!.Value;

            await _context.SaveChangesAsync(ct);
            var totalAfter = listing.Images.Count - DeleteImageIds.Count + Photos.Count;
            if (totalAfter > _imgOpt.MaxPhotos)
            {
                ModelState.AddModelError(nameof(Photos),
                    $"You reached photo limit per listing! Maximum: {_imgOpt.MaxPhotos}.");
                Listing = listing;
                ExistingImages = listing.Images.OrderBy(i => i.SortOrder).ThenBy(i => i.Id).ToList();
                return Page();
            }

            await using var tx = await _context.Database.BeginTransactionAsync(ct);
            try
                {
                //_context.Listings.Add(Listing);
                await _context.SaveChangesAsync(ct);
                if (DeleteImageIds.Count > 0)
                {
                    await _imageService.RemoveImagesAsync(listing.Id, DeleteImageIds, ct);
                }

                if (Photos is { Count: > 0 })
                {
                    await _imageService.AddImagesAsync(listing.Id, Photos, ct);
                }

                await tx.CommitAsync(ct);
                return RedirectToPage("./Details", new { id = listing.Id });
            }
            catch (Exception)
            {
                await tx.RollbackAsync(ct);
                ModelState.AddModelError(string.Empty, "Failed to save. Try again.");
                return await OnGetAsync(ct);
            }
        }

    }
}
