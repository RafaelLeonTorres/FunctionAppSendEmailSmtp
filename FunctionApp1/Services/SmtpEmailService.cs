using FunctionApp1.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1.Services
{
    internal class SmtpEmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _senderEmail;
        private readonly ILogger<SmtpEmailService> _logger;
        public SmtpEmailService(ILogger<SmtpEmailService> logger)
        {
            _smtpHost = Environment.GetEnvironmentVariable("SmtpHost");
            _smtpPort = int.Parse(Environment.GetEnvironmentVariable("SmtpPort"));
            _smtpUser = Environment.GetEnvironmentVariable("SmtpUser");
            _smtpPass = Environment.GetEnvironmentVariable("SmtpPass");
            _senderEmail = Environment.GetEnvironmentVariable("SenderEmail");
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    IsBodyHtml = true
                };

                // Agregar destinatarios
                emailMessage.To.ForEach(to => mailMessage.To.Add(to));
                emailMessage.Cc.ForEach(cc => mailMessage.CC.Add(cc));
                emailMessage.Bcc.ForEach(bcc => mailMessage.Bcc.Add(bcc));

                // Agregar adjuntos
                emailMessage.Attachments.ForEach(attachment => mailMessage.Attachments.Add(attachment));

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
