using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class PdfService : IPdfService
    {
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
    }
}








