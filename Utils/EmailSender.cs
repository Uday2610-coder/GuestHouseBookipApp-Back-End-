using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GuestHouseBookingApplication.Utils
{
    public class EmailSender
    {
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPass;

        public EmailSender(string smtpHost, int smtpPort, string smtpUser, string smtpPass)
        {
            this.smtpHost = smtpHost;
            this.smtpPort = smtpPort;
            this.smtpUser = smtpUser;
            this.smtpPass = smtpPass;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUser, smtpPass);

                var mail = new MailMessage(smtpUser, toEmail, subject, body);
                mail.IsBodyHtml = true;
                await client.SendMailAsync(mail);
            }
        }
    }
}