using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Backend.Core.Helper
{
    public static class EmailSender
    {
        private const string supportEmail = "office@tanulmanyiversenyek.hu";

        public static void SendEmail(string email, string subject, string message)
        {
            SmtpClient client = SmtpClientConfiguration();

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(supportEmail);
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            client.Send(mailMessage);
        }

        public static async void SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                SmtpClient client = SmtpClientConfiguration();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(supportEmail);
                mailMessage.Body = message;
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.SubjectEncoding = Encoding.UTF8;

                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new Exception($"SMTP exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Send mail exception: {ex.Message}");
            }
        }

        public static async void SendEmailAsync(string email, string subject, string message, byte[] data)
        {
            try
            {
                SmtpClient client = SmtpClientConfiguration();

                Stream attachment = new MemoryStream(data);

                // Create  the file attachment for this e-mail message.
                Attachment pdfAttachment = new Attachment(attachment, "dokumentum.pdf", "application/pdf");

                // Add time stamp information for the file.
                ContentDisposition disposition = pdfAttachment.ContentDisposition;
                disposition.CreationDate = DateTime.Now;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(supportEmail);
                mailMessage.To.Add(email);
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.SubjectEncoding = Encoding.UTF8;

                // Add the file attachment to this e-mail message.
                mailMessage.Attachments.Add(pdfAttachment);

                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new Exception($"SMTP exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Send mail exception: {ex.Message}");
            }
        }

        private static SmtpClient SmtpClientConfiguration()
        {
            SmtpClient client = new SmtpClient("mail.portalnekretnine.rs");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(supportEmail, "YT@5j$QC@#-3");
            client.EnableSsl = true;

            return client;
        }
    }
}
