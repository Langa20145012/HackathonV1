using Hackath_Application_API.Interfaces;
using System.Net.Mail;
using System.Net;

namespace Hackath_Application_API.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["Email:SmtpServer"];
            _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
            _smtpUsername = configuration["Email:Username"];
            _smtpPassword = configuration["Email:Password"];
            _fromEmail = configuration["Email:FromEmail"];
            _fromName = configuration["Email:FromName"];
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(to);

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
