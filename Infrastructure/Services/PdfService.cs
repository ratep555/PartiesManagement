using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Core.Interfaces;
using Infrastructure.Data;
using System.Linq;

namespace Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private readonly PartiesContext _partiesContext;
        public PdfService(PartiesContext partiesContext)
        {
            _partiesContext = partiesContext;
        }
        public void GeneratePdf(int orderNo)
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
        public void GeneratePdf1(int orderNo)
        {
            var accounts = _partiesContext.Accounts.ToList();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Arial", 15);

            gfx.DrawString("Order No: " + orderNo.ToString(), new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Red, new XPoint(170, 70));
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(50, 100), new XPoint(550, 100));

            gfx.DrawString("Customer: ", 
            new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(50, 130));

            gfx.DrawString("Amount: ", 
            new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(50, 160));

            gfx.DrawString("Please pay specified amount in one of the following accounts:", 
            new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Red, new XPoint(50, 195));

            int currentYposition_values = 230;

            for (int i = 0; i < accounts.Count(); i++)
            {
                    gfx.DrawString(accounts[i].BankName + ", " + accounts[i].IBAN,
                        new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, 
                        new XPoint(50, currentYposition_values));

                   /*  gfx.DrawString(accounts[i].IBAN,
                        new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(250, 
                        currentYposition_values)); */
                    
                    currentYposition_values += 30;

            }



            document.Save("C:\\Users\\petar\\source\\repos\\TestPDF777.pdf");        
        }
    }
}








