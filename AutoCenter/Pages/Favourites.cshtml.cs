using AutoCenter.Web.Models;
using AutoCenter.Web.Services.Favourites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AutoCenter.Web.Pages
{
    [Authorize]
    public class FavouritesModel : PageModel
    {
        private readonly IFavouriteService _favouriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouritesModel(IFavouriteService favouriteService, UserManager<ApplicationUser> userManager)
        {
            _favouriteService = favouriteService;
            _userManager = userManager;
        }

        public IReadOnlyList<Listing> Favourites { get; private set; } = Array.Empty<Listing>();

        public async Task<IActionResult> OnGetAsync(CancellationToken ct)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            Favourites = await _favouriteService.GetUserFavouritesAsync(user.Id, ct);
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAddAsync(int listingId, string? returnUrl, CancellationToken ct)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var result = await _favouriteService.AddFavouriteAsync(listingId, userId, ct);
            if (!result.Succeeded)
            {
                TempData["Error"] = result.Error;
            }

            var safeReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Page("/Listings/Index");
            return LocalRedirect(safeReturnUrl!);

        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostRemoveAsync(int listingId, CancellationToken ct)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            var result = await _favouriteService.RemoveFavouriteAsync(listingId, user.Id, ct);
            TempData[result.Succeeded ? "Success" : "Error"] =
                result.Succeeded ? "Removed from favourites." : result.Error;
            return RedirectToPage();
        }
    }
}
