using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace OrdinarioTienda.Controllers
{
    public class CorreoController
    {
        public static void EnviarCorreo(string correoDestino, string asunto, string cuerpo, string rutaPDF = null)
        {
            try
            {
                // Configuración del cliente SMTP
                SmtpClient smtp = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("112738@alumnouninter.mx", "Kooken22"),
                    EnableSsl = true
                };

                // Crear el mensaje de correo
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("112738@alumnouninter.mx"),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true
                };

                mail.To.Add(correoDestino);

                if (!string.IsNullOrEmpty(rutaPDF) && File.Exists(rutaPDF))
                {
                    Attachment pdfAdjunto = new Attachment(rutaPDF);
                    mail.Attachments.Add(pdfAdjunto);
                }
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                throw new Exception("Error al intentar enviar el correo. " + ex.Message, ex);
            }
        }
    }
}


