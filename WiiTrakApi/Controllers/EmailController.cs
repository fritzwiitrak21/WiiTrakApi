/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.AspNetCore.Mvc;
using WiiTrakApi.DTOs;
using WiiTrakApi.Services.Contracts;
using System.Threading.Tasks;

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
