using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Helpers;
using API.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace API.Application.Services
{
    public class EmailService : IEmailService
    {
        readonly EmailSettings _settings;
        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task<bool> SendEmail(EmailMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Crypto Training", "crptotrain"));
            emailMessage.To.Add(new MailboxAddress("", message.Email.Trim()));
            emailMessage.Subject = message.Title;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Message
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(_settings.Email, _settings.Password);
                client.Send(emailMessage);
                await client.DisconnectAsync(true);
            }
            return true;
        }
    }
}