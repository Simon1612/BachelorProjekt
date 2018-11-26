using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.IO;

namespace DentalResearchApp.Code.Impl
{
    public class MailHelper
    {
        //TODO: Change to AU's mailserver.
        private string smtpServer = "smtp.gmail.com";
        private int sslPort = 465;
        private int tlsPort = 587;
        private SmtpClient client;

        public MailHelper()
        {
            client = new SmtpClient();

            //TODO: Add actual account for @dent.au.dk

            var User = "audenttest@gmail.com";
            var Password = "Virknu123!";

            client.Connect(smtpServer, sslPort, SecureSocketOptions.SslOnConnect);
            client.Authenticate(User, Password);
        }

        public void SendMail(string receiverAdress, string messageSubject, string messageBody)
        {
            var path = @"wwwroot\img\auLogo.png";
            //MailMessage mailMessage = new MailMessage();
            var mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress("audenttest@gmail.com"));
            mailMessage.To.Add(new MailboxAddress(receiverAdress));
            mailMessage.Subject = messageSubject;

            var body = new TextPart("plain")
            {
                Text = messageBody
            };

            var attachment = new MimePart("image", "png")
            {
                ContentObject = new ContentObject(File.OpenRead(path), ContentEncoding.Base64),
                //TODO: FIX
                ContentDisposition = new ContentDisposition(ContentDisposition.FormData),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(path),
                
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            mailMessage.Body = multipart;

            client.Send(mailMessage);
            client.Disconnect(true);
        }
    }
}
