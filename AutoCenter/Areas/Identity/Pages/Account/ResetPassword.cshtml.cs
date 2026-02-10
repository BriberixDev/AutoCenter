using AutoCenter.Web.Services.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoCenter.Web.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IAccountService _accountService;
        public ResetPasswordModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [BindProperty]
        public InputModel Input { get; set; } = new();
        public IActionResult OnGet(string email,string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return RedirectToPage("/Account/ForgotPassword");
            Input ??= new InputModel();
            Input.Email= email;
            Input.Code= token;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
                if (!ModelState.IsValid)
                {
                return Page();
                }
                var result = await _accountService.ResetPasswordAsync(Input.Email, Input.Code, Input.Password);
                if(result.Succeeded)
                {
                    TempData["StatusMessage"] = "Your password has been reset successfully.";
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return Page();
        }
    }
    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;


        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8,
        ErrorMessage = "The password must be at least {2} and at max {1} characters long.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}