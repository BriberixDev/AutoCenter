using AutoCenter.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace AutoCenter.Web.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly IEmailService _email;
        public AccountService(UserManager<ApplicationUser> userManager, IEmailService email)
        {
            _users = userManager;
            _email = email;
        }
        public async Task SendPasswordResetLinkAsync(string email, Func<string, string> builderResetLink)
        {
            var user = await _users.FindByEmailAsync(email);
            if (user is null) return;

            if (_users.Options.SignIn.RequireConfirmedEmail &&
                !await _users.IsEmailConfirmedAsync(user))
            {
                return;
            }
            var firstName = user.FirstName??"User";
            //Generate a secure token for password reset
            var token = await _users.GeneratePasswordResetTokenAsync(user);

            //Encode the token to make it URL-safe
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); 
            var resetLink = builderResetLink(encodedToken);

            await _email.SendPasswordResetEmailAsync(email, firstName, resetLink);
            
        }
        public async Task<IdentityResult> ResetPasswordAsync(string email, string encodedToken, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(encodedToken) || string.IsNullOrWhiteSpace(newPassword))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid input." });
            var user = await _users.FindByEmailAsync(email);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid token or email." });
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedToken));
            return await _users.ResetPasswordAsync(user, token, newPassword);
        }
    }
}
