using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WiiTrakApi.Services.Contracts;
using WiiTrakApi.DTOs;
using WiiTrakApi.Cores;
using System;
using System.Net;
using System.Net.Mail;

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
            var HostUrl = _mailSettings.HostUrl;
            var Key = Core.Base64Encrypt(Request.MailTo);
            var ResetUrl = string.Concat(HostUrl, "/reset-password?Key=", Key, "&Key1=" + Request.UserId);


            var filepath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Mail", "ForgotPassword.htm");
            var htmlcontent = "<h1>WiiTrak</h1>";
            if (File.Exists(filepath))
            {
                htmlcontent = File.ReadAllText(filepath);
                htmlcontent = htmlcontent.Replace("##ResetUrl##", ResetUrl);
            }

            SmtpClient SmtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port);

            // set smtp-client with basicAuthentication
            SmtpClient.UseDefaultCredentials = false;
            NetworkCredential basicAuthenticationInfo = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            SmtpClient.Credentials = basicAuthenticationInfo;

            // add from,to mailaddresses
            MailAddress from = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            MailAddress to = new MailAddress(Request.MailTo, Request.Name);
            MailMessage Mail = new System.Net.Mail.MailMessage(from, to);



            // set subject and encoding
            Mail.Subject = "Set your new WiiTrak password";
            Mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            Mail.Body = htmlcontent;
            Mail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            Mail.IsBodyHtml = true;

        }

        public async Task SendLoginCredentialsMailAsync(MailRequest Request)
        {

            var HostUrl = _mailSettings.HostUrl;
            var filepath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Mail", "LoginCredentials.htm");
            var htmlcontent = "<h1>WiiTrak</h1>";
            if (File.Exists(filepath))
            {
                htmlcontent = File.ReadAllText(filepath);
                htmlcontent = htmlcontent.Replace("##HostUrl##", HostUrl);
                htmlcontent = htmlcontent.Replace("##Email##", Request.MailTo);//UserName
                htmlcontent = htmlcontent.Replace("##Password##", Core.DefaultPassword);//Password

            }

            SmtpClient SmtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port);

            // set smtp-client with basicAuthentication
            SmtpClient.UseDefaultCredentials = false;
            NetworkCredential basicAuthenticationInfo = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            SmtpClient.Credentials = basicAuthenticationInfo;

            // add from,to mailaddresses
            MailAddress from = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            MailAddress to = new MailAddress(Request.MailTo, Request.Name);
            MailMessage Mail = new System.Net.Mail.MailMessage(from, to);


            // set subject and encoding
            Mail.Subject = "Login Credentials for your WiiTrak account";
            Mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            Mail.Body = htmlcontent;
            Mail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            Mail.IsBodyHtml = true;

            SmtpClient.Send(Mail);

        }
    }
}
