using System.Threading.Tasks;
using WiiTrakApi.DTOs;

namespace WiiTrakApi.Services.Contracts
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(MailRequest Request);
        Task SendLoginCredentialsMailAsync(MailRequest Request);
    }
}
