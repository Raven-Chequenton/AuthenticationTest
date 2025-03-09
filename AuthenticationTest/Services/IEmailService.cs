using System.Threading.Tasks;

namespace AuthenticationTest.Services
{
    public interface IEmailService
    {
        Task ProcessEmailsAsync();
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
