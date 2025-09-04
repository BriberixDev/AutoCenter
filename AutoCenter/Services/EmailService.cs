using AutoCenter.Web.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AutoCenter.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SmtpSettings> smtpSetting;

        public EmailService(IOptions<SmtpSettings> smtpSetting)
        {
            this.smtpSetting = smtpSetting;
        }
        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from, to, subject, body);

            using (var emailClient = new SmtpClient(smtpSetting.Value.Host, smtpSetting.Value.Port))
            {
                emailClient.Credentials = new NetworkCredential(
                    smtpSetting.Value.User,
                    smtpSetting.Value.Password);
                await emailClient.SendMailAsync(message);
            }
        }
    }
}
