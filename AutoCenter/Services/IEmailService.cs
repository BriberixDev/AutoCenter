
namespace AutoCenter.Web.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body,bool isHtml=false);
        Task SendPasswordResetEmailAsync(string toEmail,string firstName, string resetLink);
    }
}