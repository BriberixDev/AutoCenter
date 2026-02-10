using AutoCenter.Web.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;

namespace AutoCenter.Web.Services.Account
{
    public interface IAccountService
    {
        Task SendPasswordResetLinkAsync(string email,Func<string,string>builderResetLink);
        Task<IdentityResult> ResetPasswordAsync(string email,string token,string newPassword);

    }
}
