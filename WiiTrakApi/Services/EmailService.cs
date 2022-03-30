using MimeKit;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using WiiTrakApi.Services.Contracts;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;


namespace WiiTrakApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettingsDto _mailSettings;
        
        
        public EmailService(IOptions<MailSettingsDto> mailSettings)
        {
            
            _mailSettings = mailSettings.Value;
        }
        public async Task SendPasswordResetEmailAsync(MailRequest Request)
        {
            try
            {

            
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse("sivasatheesh.mn@dsrc.co.in"));
            //email.To.Add(MailboxAddress.Parse(mailRequest.MailTo));
            email.Subject = "Set your new WiiTrak password";

            var HostUrl = _mailSettings.HostUrl;
            var Key = Core.Base64Encrypt(Request.MailTo);
            var ResetUrl = string.Concat(HostUrl,"/reset-password?Key=", Key, "&Key1=" + Request.UserId);
            var builder = new BodyBuilder();
                  
            var filepath = Path.Combine(System.IO.Directory.GetCurrentDirectory(),  "Mail", "ForgotPassword.htm");
            var htmlcontent = "<h1>WiiTrak</h1>";
            if (File.Exists(filepath))
            {
                htmlcontent = File.ReadAllText(filepath);
                htmlcontent = htmlcontent.Replace("##ResetUrl##", ResetUrl);
            }

            builder.HtmlBody = htmlcontent;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task SendLoginCredentialsMailAsync(MailRequest Request)
        {
            try { 
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse("sivasatheesh.mn@dsrc.co.in"));
            //email.To.Add(MailboxAddress.Parse(mailRequest.MailTo));
            email.Subject = "Login Credentials for your WiiTrak account";

            var builder = new BodyBuilder();
            var HostUrl = _mailSettings.HostUrl;

            var filepath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Mail", "LoginCredentials.htm");
            var htmlcontent = "<h1>WiiTrak</h1>";
            if (File.Exists(filepath))
            {
                htmlcontent = File.ReadAllText(filepath);
                htmlcontent = htmlcontent.Replace("##HostUrl##", HostUrl);
                htmlcontent = htmlcontent.Replace("##Email##", Request.Name);//UserName
                htmlcontent = htmlcontent.Replace("##Password##", Core.DefaultPassword);//Password

            }

            builder.HtmlBody = htmlcontent;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
