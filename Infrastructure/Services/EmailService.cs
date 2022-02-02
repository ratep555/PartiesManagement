using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private UserManager<ApplicationUser> _userManager;
        public EmailService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _config["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("petar.sardelic@outlook.com", "Doctor Auth Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);        
        }
        
        public async Task ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);
        }

        public void GeneratePdf(int orderNo, decimal amount, string firstName, string lastName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Arial", 15);

            gfx.DrawString("Order No:", font, XBrushes.Black, new XPoint(50, 100));
            gfx.DrawString(orderNo.ToString(), font, XBrushes.Black, new XPoint(120, 100));

            gfx.DrawString("Customer:", font, XBrushes.Black, new XPoint(50, 130));
            gfx.DrawString("Pero Perić", font, XBrushes.Black, new XPoint(124, 130));

            gfx.DrawString("Amount:", font, XBrushes.Black, new XPoint(50, 160));
            gfx.DrawString(orderNo.ToString(), font, XBrushes.Black, new XPoint(110, 160));

            gfx.DrawString("Account No:", font, XBrushes.Black, new XPoint(50, 190));
            gfx.DrawString("777777-777777", font, XBrushes.Black, new XPoint(138, 190));

            document.Save("C:\\Users\\petar\\source\\repos\\TestPDF2.pdf");        
        }

        public async Task SendEmailAsync1(string toEmail, string subject, string content)
        {
            var apiKey = _config["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("petar.sardelic@outlook.com", "Doctor Auth Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            using (var fileStream = File.OpenRead("C:\\Users\\petar\\source\\repos\\TestPDF2.pdf"))
            {
                await msg.AddAttachmentAsync("TestPDF2.pdf", fileStream);
                var response = await client.SendEmailAsync(msg);
            }    
        }
    }
}











