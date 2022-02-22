using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
        Task SendEmailAsync1(string toEmail, string subject, string content, int orderNo);
        Task ConfirmEmailAsync(string email, string token);
        void GeneratePdf(int orderNo, decimal amount, string firstName, string lastName);
        void GeneratePdf1(int orderNo, decimal amount, string firstName, string lastName);
        void GeneratePdf2(int orderNo, decimal amount, string name);
        Task SendEmailAsync2(string toEmail, string subject, string content, int orderNo);


    }
}