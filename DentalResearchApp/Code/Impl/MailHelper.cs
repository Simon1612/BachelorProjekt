using System.Net;
using System.Net.Mail;

namespace DentalResearchApp.Code.Impl
{
    public class MailHelper
    {
        //TODO: Change to AU's mailserver.
        private string smtpServer = "smtp.gmail.com";
        //private int sslPort = 465;
        private int tlsPort = 587;
        private SmtpClient client;

        public MailHelper()
        {
            client = new SmtpClient(smtpServer);
            client.Port = tlsPort;
            client.UseDefaultCredentials = false;

            //TODO: Add actual account for @dent.au.dk
            client.Credentials = new NetworkCredential("audenttest@gmail.com", "Virknu123!");
            client.EnableSsl = true;
        }

        public async void SendMail(string receiverAdress, string messageSubject, string messageBody)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("audenttest@gmail.com");
            mailMessage.To.Add(receiverAdress);
            mailMessage.Body = messageBody;
            mailMessage.Subject = messageSubject;

            await client.SendMailAsync(mailMessage);
        }
    }
}
