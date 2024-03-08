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
                //Credentials = new NetworkCredential("12446363989@edu.udesc.br", "Jv5626$$"), //trocarEmail, fazer um outlook só pra isso
                Credentials = new NetworkCredential("botuoficial@outlook.com", "Botuehgolfinho"), //trocarEmail, fazer um outlook só pra isso
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
                // Handle exception
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
