using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace apppacheco
{
    class Pdf
    {
        private void setDocumentStyle(ref Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Calibri";
            style = document.Styles[StyleNames.Header];
            //style.Section.PageSetupHeaders.EvenPage.AddTable();
            style.ParagraphFormat.AddTabStop("1cm", TabAlignment.Center);
            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("1cm", TabAlignment.Center);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Bold = false;
            style.ParagraphFormat.Font.Size = 7;
            style = document.Styles[StyleNames.Heading1];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Bold = true;
            style = document.Styles[StyleNames.Heading2];
            style.ParagraphFormat.Font.Bold = false;
            style = document.Styles[StyleNames.Heading3];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Bold = true;
            style.ParagraphFormat.Font.Size = 6;
            style = document.Styles[StyleNames.Heading4];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.Font.Bold = false;
            style.ParagraphFormat.Font.Size = 8;
            style = document.Styles[StyleNames.Heading5];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Font.Bold = false;
            style.ParagraphFormat.Font.Size = 7;
        }

        public string crearPdf(string[] fotos)
        {

            Document document = new Document();
            PdfDocument pdfDocument = new PdfDocument();
            document.DefaultPageSetup.PageFormat = PageFormat.A4;
            document.DefaultPageSetup.LeftMargin = "2cm";
            document.DefaultPageSetup.TopMargin = "2cm";
            setDocumentStyle(ref document);
            string footerText = "LISTADO DE CODIGO DE BARRAS POR PROPIETARIO CON NIT Y NUMERO DE UNIDAD ";
            setDocumentHeaders2(
                 ref document,
                 footerText
            );
            foreach (string foto in fotos)
            {
                document.LastSection.AddImage(foto);
            }
            PdfDocumentRenderer pdfDocumentRenderer = new PdfDocumentRenderer(false, PdfFontEmbedding.Always);
            pdfDocumentRenderer.Document = document;
            pdfDocumentRenderer.RenderDocument();
            string filename = "archivopdf\\ARCHIVO_CODIGO_DE_BARRAS.pdf";
            pdfDocumentRenderer.PdfDocument.Save(filename);
            return filename;
            }
        
        public void setDocumentHeaders2(ref Document document, string footerText)
        {
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            //section.PageSetup.HeaderDistance = "1cm";
            document.DefaultPageSetup.HeaderDistance = "1cm";
            //document.DefaultPageSetup.TopMargin = "2.5cm";
            document.DefaultPageSetup.TopMargin = "3.5cm";
            section.PageSetup.StartingNumber = 1;
            HeaderFooter headerPrimaryPage = section.Headers.Primary;
            Table headerTable = headerPrimaryPage.AddTable();
            HeaderFooter headerEvenPage = section.Headers.EvenPage;
            Table headerTableEvenPage = headerEvenPage.AddTable();
            Paragraph footerParagraphfirstPage = section.Headers.Primary.AddParagraph();
            footerParagraphfirstPage.AddText(footerText);
            footerParagraphfirstPage.Style = StyleNames.Header;
            Paragraph footerParagraphEvenPage = section.Headers.EvenPage.AddParagraph();
            footerParagraphEvenPage.AddText(footerText);
            footerParagraphEvenPage.Style = StyleNames.Header;
        }
    }
}

