using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("botuoficial@outlook.com", "Botuehgolfinho"),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MailMessage("botuoficial@outlook.com", toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
