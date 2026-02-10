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
        public async Task SendAsync(string to, string subject, string body,bool isHtml=false)
        {
            var from = new MailAddress(smtpSetting.Value.FromEmail, smtpSetting.Value.FromName);

            using var message = new MailMessage
            {
                From = from,
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            message.To.Add(to);

            using var emailClient = new SmtpClient(smtpSetting.Value.Host, smtpSetting.Value.Port)
            {
                Credentials = new NetworkCredential(smtpSetting.Value.User, smtpSetting.Value.Password),
                EnableSsl = true
            };

            await emailClient.SendMailAsync(message);
        }
        public async Task SendPasswordResetEmailAsync(string toEmail, string firstName, string resetLink)
        {
            var safeName = WebUtility.HtmlEncode(string.IsNullOrWhiteSpace(firstName) ? "User" : firstName);
            var linkForHref = resetLink;
            var linkForText = WebUtility.HtmlEncode(resetLink);

            var subject = "Password reset";

            var html = $@"
            <html>
              <body style='font-family: Arial, sans-serif; background:#f4f6f8; margin:0; padding:20px;'>
                <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                  <h2 style='color:#333; margin-top:0;'>Password reset</h2>
                  <p style='font-size:16px; color:#555;'>Hi {safeName},</p>
                  <p style='font-size:16px; color:#555;'>
                    We received a request to reset your password. Click the button below to choose a new one.
                  </p>

                  <p style='text-align:center; margin:24px 0;'>
                    <a href='{linkForHref}'
                       style='background:#0d6efd; color:#fff; padding:12px 24px; border-radius:6px;
                              text-decoration:none; font-weight:bold; display:inline-block;'>
                      Reset password
                    </a>
                  </p>

                  <p style='font-size:12px; color:#777; word-break:break-all;'>
                    If the button doesn’t work, copy this link:<br/>
                    <a href='{linkForHref}'>{linkForHref}</a>
                  </p>

                  <p style='font-size:13px; color:#777;'>
                    If you didn't request this, you can ignore this email.
                  </p>

                  <p style='font-size:12px; color:#999; margin-top:30px;'>
                    &copy; {DateTime.UtcNow.Year} AutoCenter
                  </p>
                </div>
              </body>
            </html>";

            await SendAsync(toEmail, subject, html, isHtml: true);
        }
    }
}
