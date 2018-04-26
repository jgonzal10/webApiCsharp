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
using System.Data;
using System.Web.UI;
using System.Text;
using System.Net.Mail;

namespace JGOReports.Controllers
{
    public class JgoReportsController : ApiController
    {
        public JgoReportsController() { }




        [Route("api/JgoReports/CreatePDF")]
        [HttpGet]
        [ActionName("CreatePDF")]
        public HttpResponseMessage CreatePDF(string report)
        {
            var stream = CreatePdfString(report);

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


        private Stream CreatePdfString(string report)
        {
            using (var document = new Document(PageSize.A4, 50, 50, 25, 25))
            {
                var output = new MemoryStream();

                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;

                document.Open();
                document.Add(new Paragraph(report));
                document.Close();

                output.Seek(0, SeekOrigin.Begin);

                return output;
            }
        }





        [Route("api/JgoReports/generatePdfsend")]
        [HttpGet]
        [ActionName("generatePdfsend")]
        public string generatePdfsend()
        {
            string retorno = "ok";
            try
            {

                //Define os dados do e-mail
                string nomeRemetente = "JGO REPORTS";
                string emailRemetente = "jgo@g.com";
                string senha = "abc1234";

                //Host da porta SMTP
                string SMTP = "smtp.jgo.com.br";

                string emailDestinatario = "myemail@hotil.com";


                string assuntoMensagem = "INVOIC";
                string conteudoMensagem = "200";
          

                var doc = new Document();
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

                doc.Open();
                doc.Add(new Paragraph("Report"));
                doc.Add(new Paragraph(conteudoMensagem));

                writer.CloseStream = false;
                doc.Close();
                memoryStream.Position = 0;

                System.Net.Mail.MailMessage objEmail = new System.Net.Mail.MailMessage();

               
                objEmail.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");

              
                objEmail.To.Add(emailDestinatario);

               
                objEmail.Priority = System.Net.Mail.MailPriority.Normal;

               
                objEmail.IsBodyHtml = true;

                objEmail.Subject = assuntoMensagem;

                
                objEmail.Body = conteudoMensagem;

                objEmail.Attachments.Add(new Attachment(memoryStream, "invoice.pdf"));
                
                objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");


              
                System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient();

                
                objSmtp.Credentials = new System.Net.NetworkCredential(emailRemetente, senha);
                objSmtp.Host = SMTP;
                objSmtp.Port = 587;

                try
                {
                    objSmtp.Send(objEmail);
                    retorno = "ok";
                }
                catch (Exception ex)
                {
                    retorno = " Issues to send e-mail. Error = " + ex.Message;
                }
                finally
                {
                    //excluímos o objeto de e-mail da memória
                    objEmail.Dispose();
                    //anexo.Dispose();
                }
            }
            catch (Exception ex)
            {
                retorno = "here" + ex.Message;
            }
            return retorno;
        }


    }
}
