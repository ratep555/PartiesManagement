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
using Infrastructure.Data;
using System.Linq;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private UserManager<ApplicationUser> _userManager;
        private readonly PartiesContext _context;
        public EmailService(IConfiguration config, UserManager<ApplicationUser> userManager, PartiesContext context)
        {
            _context = context;
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
            gfx.DrawString(lastName + ", " + firstName, font, XBrushes.Black, new XPoint(124, 130));

            gfx.DrawString("Amount:", font, XBrushes.Black, new XPoint(50, 160));
            gfx.DrawString(amount.ToString(), font, XBrushes.Black, new XPoint(110, 160));

            gfx.DrawString("Account No:", font, XBrushes.Black, new XPoint(50, 190));
            gfx.DrawString("777777-777777", font, XBrushes.Black, new XPoint(138, 190));

            document.Save("C:\\Users\\petar\\source\\repos\\" + orderNo.ToString() + ".pdf");
        }

        public void GeneratePdf1(int orderNo, decimal amount, string firstName, string lastName)
        {
            var accounts = _context.Accounts.ToList();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Arial", 15);

            gfx.DrawString("Order No: " + orderNo.ToString(), new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Red, new XPoint(170, 70));
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(50, 100), new XPoint(550, 100));

            gfx.DrawString("Customer: " + lastName + ", " + firstName,
            new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(50, 130));

            gfx.DrawString("Amount: " + amount.ToString(),
            new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(50, 160));

            gfx.DrawString("Please pay specified amount in one of the following accounts:",
            new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Red, new XPoint(50, 195));

            int currentYposition_values = 230;

            for (int i = 0; i < accounts.Count(); i++)
            {
                gfx.DrawString(accounts[i].BankName + ", " + accounts[i].IBAN,
                    new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black,
                    new XPoint(50, currentYposition_values));

                currentYposition_values += 30;
            }
            
            document.Save("C:\\Users\\petar\\source\\repos\\" + orderNo.ToString() + ".pdf");
        }


        public async Task SendEmailAsync1(string toEmail, string subject, string content, int orderNo)
        {
            var apiKey = _config["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("petar.sardelic@outlook.com", "Doctor Auth Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);

            using (var fileStream = File.OpenRead("C:\\Users\\petar\\source\\repos\\" + orderNo.ToString() + ".pdf"))
            {
                await msg.AddAttachmentAsync("TestPDF2.pdf", fileStream);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}











