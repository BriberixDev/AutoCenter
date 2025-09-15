using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Identity;
        using Microsoft.AspNetCore.Mvc;
        using Microsoft.AspNetCore.Mvc.RazorPages;

        namespace AutoCenter.Web.Areas.Identity.Pages.Account
        {
            public class ConfirmEmailModel : PageModel
            {
                [BindProperty]
                public string Message { get; set; } = string.Empty;

                private readonly UserManager<ApplicationUser> userManager;

                public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
                {
                    this.userManager = userManager;
                }
                public async Task<IActionResult> OnGetAsync(string userId, string token)
                {
                        var user = await this.userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            var result = await this.userManager.ConfirmEmailAsync(user, token);
                            if (result.Succeeded)
                            {
                                this.Message = "Email adress is successfully confirmed,you can now try to login";
                                return Page();
                            }
                        }
                    this.Message = "Failed to validate email.";
                    return Page();
                }
            }
        }
