using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http.Headers;

namespace JGOReports.Controllers
{
    public class JgoReportsController : ApiController
    {
        public JgoReportsController() { }


        [Route("api/JgoReports/GetPDF")]
        [HttpGet]
        [ActionName("GetPDF")]
        public byte[] GetPDF()
        {
            string pHTML = "pdf <b>JGO Reports</b>";
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document
            doc.Open();
            htmlWorker.StartDocument();

            // 5: parse the html into the document
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }

        [Route("api/JgoReports/ExampleOne")]
        [HttpGet]
        [ActionName("ExampleOne")]
        public HttpResponseMessage ExampleOne()
        {
            var stream = CreatePdf();

            return new HttpResponseMessage
            {
                Content = new StreamContent(stream)
                {
                    Headers =
            {
                ContentType = new MediaTypeHeaderValue("application/pdf"),
                ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "report.pdf"
                }
            }
                },
                StatusCode = HttpStatusCode.OK
            };
        }


        private Stream CreatePdf()
        {
            using (var document = new Document(PageSize.A4, 50, 50, 25, 25))
            {
                var output = new MemoryStream();

                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;

                document.Open();
                document.Add(new Paragraph("JGO Reports"));
                document.Close();

                output.Seek(0, SeekOrigin.Begin);

                return output;
            }
        }
    }
}
