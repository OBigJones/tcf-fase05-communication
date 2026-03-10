using Communication.Application.Abstractions.Clients;
using Communication.Domain.ValueObjects;
using Communication.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Communication.Infrastructure.Clients
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IOptions<SmtpSettings> settings, ILogger<SmtpEmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailMessage message)
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = false
            };

            if (!string.IsNullOrWhiteSpace(_settings.User))
            {
                client.Credentials = new NetworkCredential(_settings.User, _settings.Password);
                client.EnableSsl = true;
            }

            var mail = new MailMessage(_settings.From, message.To, message.Subject, message.Body);

            try
            {
                _logger.LogInformation("Sending email to {Email}", message.To);
                await client.SendMailAsync(mail);
                _logger.LogInformation("Email sent to {Email}", message.To);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", message.To);
                throw;
            }
        }
    }
}
