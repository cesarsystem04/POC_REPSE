using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using GhostscriptSharp;
using GhostscriptSharp.Settings;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Mvc;
using Spire.Pdf;
using ZXing;
using ZXing.Common;

using Path = System.IO.Path;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace POC_REPSE.Utils;
internal class PDF
{
    public void ReadText(byte[] Base64)
    {
        byte[] fileBytes =Base64;  
        // Create a new instance of PdfReader
        PdfReader reader = new PdfReader(fileBytes);

        // Get the number of pages in the PDF file
        int numPages = reader.NumberOfPages;

        // Create a new instance of StringBuilder to store the contents of the PDF file
        StringBuilder text = new StringBuilder();

        // Loop through each page of the PDF file
        for (int i = 1; i <= numPages; i++)
        {
            // Get the content of the current page
            string pageText = PdfTextExtractor.GetTextFromPage(reader, i);

            // Append the content of the page to the StringBuilder
            text.Append(pageText);
        }

        // Close the PDF file
        reader.Close();

        // Display the contents of the PDF file in the console
        Console.WriteLine(text.ToString());
    }
    public void ReadQR(byte[] Base64)
    {
        //PdfDocument doc = new PdfDocument();
        //doc.LoadFromFile(@"C:\Users\prave\Desktop\my.pdf");
        //for (int i = 0; i < doc.Pages.Count; i++)
        //{
        //    PdfPageBase page = doc.Pages[i];
        //    System.Drawing.Image[] images = page.ExtractImages();
        //    outputPath = Path.Combine(outputPath, String.Format(@"{0}.jpg", i));
        //    foreach (System.Drawing.Image image in images)
        //    {
        //        string QRCodeString = GetQRCodeString(image, outputPath);
        //        if (QRCodeString == "")
        //        {
        //            continue;
        //        }
        //        break;
        //    }
        //}


        // Lee el contenido del PDF en una matriz de bytes
        byte[] pdfContent = Base64;

        // Crea un objeto de decodificador de QR
        var qrCodeReader = new BarcodeReader();

        //   var source = new BitmapLuminanceSource(barcodeBitmap);
        // Configura el decodificador para que solo busque códigos QR
        qrCodeReader.Options.PossibleFormats = new BarcodeFormat[] { BarcodeFormat.QR_CODE };
        
        // Analiza el contenido del PDF en busca de un código QR
        var qrCodeResult = qrCodeReader.Decode(pdfContent);

        // Si se ha encontrado un código QR, muestra su contenido en pantalla
        if (qrCodeResult != null)
        {
            Console.WriteLine(qrCodeResult.Text);
        }
        else
        {
            Console.WriteLine("No se ha encontrado ningún código QR en el PDF");
        }

    }
    //private static string GetQRCodeString(System.Drawing.Image img, string outPutPath)
    //{
    //    img.Save(outPutPath, System.Drawing.Imaging.ImageFormat.Jpeg);
    //    string scaningResult = Spire.Barcode.BarcodeScanner.ScanOne(outPutPath);
    //    File.Delete(outPutPath);
    //    return scaningResult;
    //}
}
