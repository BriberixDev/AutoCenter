using AutoCenter.Web.Models;
using AutoCenter.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace AutoCenter.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;

        public RegisterModel(UserManager<ApplicationUser> userManager,IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }
        [BindProperty]
        public RegisterViewModel Input { get; set; } = new RegisterViewModel();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var user = new ApplicationUser
            {
                UserName = Input.UserName,
                Email = Input.Email,
            };
            var result = await this.userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.PageLink(pageName: "./ConfirmEmail", values: new { userId = user.Id, token = confirmationToken });

                await emailService.SendAsync(user.Email,
                    "Please confirm your email",
                    $"Please click on this link to confirm your email adress: {confirmationLink}");

                
                return RedirectToPage("./Login");
            }
            else
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return Page();
            }
        }
    }
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email adress.")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
