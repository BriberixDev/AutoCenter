using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoCenter.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        [BindProperty]
        public CredentialViewModel Input { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await signInManager.PasswordSignInAsync(this.Input.UserName, this.Input.Password, this.Input.RememberMe, false);
            if(result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "You are locked out.");
                }
                else
                {
                    ModelState.AddModelError("Login", "Failed to login.");
                }
                return Page();
            }
            
        }
        public class CredentialViewModel
        {
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; } = string.Empty;
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Remember Me")]
            public bool RememberMe { get; set; }
        }
    }
}