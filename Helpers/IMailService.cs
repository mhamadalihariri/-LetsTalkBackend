using LetsTalkBackend.Models;
using System.Threading.Tasks;

namespace LetsTalkBackend.Helpers
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequest request);

    }
}
