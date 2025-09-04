using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoCenter.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Verify the credential
            if (Input.UserName == "admin" && Input.Password == "Password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email,"admin@mywebsite.com"),
                    new Claim("Admin","true")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Input.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal,authProperties);
                return RedirectToPage("/Index");
            }

            return Page();
        }
        public class InputModel
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
