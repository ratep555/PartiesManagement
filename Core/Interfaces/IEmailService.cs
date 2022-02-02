using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
        Task SendEmailAsync1(string toEmail, string subject, string content);
        Task ConfirmEmailAsync(string email, string token);
        void GeneratePdf(int orderNo, decimal amount, string firstName, string lastName);

    }
}