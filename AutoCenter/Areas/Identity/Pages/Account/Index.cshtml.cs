using System.ComponentModel.DataAnnotations;
using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AutoCenter.Web.Areas.Identity.Pages.Account
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AutoCenterDbContext _context;

        public IndexModel(UserManager<ApplicationUser> userManager, AutoCenterDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser? CurrentUser { get; private set; }
        public string? ProfileImageDataUri { get; private set; }
        public List<Listing> MyListings { get; set; } = new();

        [BindProperty]
        public EditInputModel Input { get; set; } = new();

        public class EditInputModel
        {
            [StringLength(64)]
            public string? FirstName { get; set; }

            [StringLength(64)]
            public string? LastName { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            CurrentUser = user;
            Input = new EditInputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            SetImage(user);
            await LoadUserListingsAsync(user.Id);
            return Page();
        }

        // Update first/last name
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload user and image to keep page state consistent
                var u0 = await _userManager.GetUserAsync(User);
                CurrentUser = u0;
                SetImage(u0!);
                await LoadUserListingsAsync(u0!.Id);
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            user.FirstName = Input.FirstName?.Trim() ?? string.Empty;
            user.LastName = Input.LastName?.Trim() ?? string.Empty;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                CurrentUser = user;
                SetImage(user);
                await LoadUserListingsAsync(user.Id);
                return Page();
            }

            TempData["StatusMessage"] = "Profile Saved.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUploadAvatarAsync(IFormFile? avatar)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            if (avatar is null || avatar.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "File not selected.");
                CurrentUser = user;
                await LoadUserListingsAsync(user.Id);
                SetImage(user);
                return Page();
            }

            // Restriction: <= 2 MB, only JPEG/PNG/WebP
            const long maxBytes = 2 * 1024 * 1024;
            if (avatar.Length > maxBytes)
            {
                ModelState.AddModelError(string.Empty, "File too big (max. 2 MB).");
                CurrentUser = user;
                SetImage(user);
                await LoadUserListingsAsync(user.Id);
                return Page();
            }

            var contentType = avatar.ContentType?.ToLowerInvariant() ?? "";
            var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowed.Contains(contentType))
            {
                ModelState.AddModelError(string.Empty, "Only JPG/PNG/WebP formats are allowed.");
                CurrentUser = user;
                SetImage(user);
                await LoadUserListingsAsync(user.Id);
                return Page();
            }

            using var ms = new MemoryStream();
            await avatar.CopyToAsync(ms);
            user.ProfilePicture = ms.ToArray();

            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
            {
                foreach (var e in update.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                CurrentUser = user;
                await LoadUserListingsAsync(user.Id);
                SetImage(user);
                return Page();
            }

            TempData["StatusMessage"] = "Avatar updated.";
            return RedirectToPage();
        }

        private void SetImage(ApplicationUser user)
        {
            if (user.ProfilePicture is { Length: > 0 })
            {
                // Convert profile picture bytes into a Base64 data URI for inline rendering
                ProfileImageDataUri = $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}";
            }
        }
        private async Task LoadUserListingsAsync(string userId)
        {
            MyListings = await _context.Listings.AsNoTracking()
                .Include(l=> l.Vehicle)
                    .ThenInclude(v => v.Brand)
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.CarModel)
                .Where(l => l.OwnerId == userId)
                .ToListAsync();
        }
    }
}
