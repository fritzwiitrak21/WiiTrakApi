using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Services.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;

namespace WiiTrakApi.Controllers
{
    //[Authorize]
    [Route("api/email")]
    [ApiController]
    
    public class EmailController : ControllerBase
    {
        private readonly IEmailService MailService;

        public EmailController(IEmailService mailservice)
        {
            MailService = mailservice;
        }

        [HttpPut]
        public async Task<IActionResult> SendEmail(MailRequest Request)
        {
            if (Request.IsForgotPasswordMail)
            {
                await MailService.SendPasswordResetEmailAsync(Request);
            }
            else
            {
                await MailService.SendLoginCredentialsMailAsync(Request);
            }
            return Ok();
        }

        
    }
}
