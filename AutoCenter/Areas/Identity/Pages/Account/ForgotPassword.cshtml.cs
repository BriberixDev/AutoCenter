using AutoCenter.Web.Services.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AutoCenter.Web.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IAccountService _accountService;
        public ForgotPasswordModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [BindProperty] //Attribute to bind the form data to the property
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _accountService.SendPasswordResetLinkAsync(Email,token =>
            {
            var url = Url.Page(
                "/Account/ResetPassword",
                    pageHandler:null,
                    values: new { area="Identity", email=Email, token},
                    protocol:Request.Scheme);
            return url!;
            }
            );
            TempData["StatusMessage"] = "If the email exists, we sent a reset link.";
            return Page();
        }
    }
}
