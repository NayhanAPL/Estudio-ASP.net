using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using versión_5_asp.services;
using System.Collections.Generic;

namespace versión_5_asp
{
    internal class ApplicationEmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public ApplicationEmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = CreateEmailMessage(new Message(to:new List<string>() { email }, subject, htmlMessage));
            await Send(emailMessage);
            /*var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Trueque", "gervisrosabal@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = htmlMessage };

            using (var client = new SmtpClient())
            {
                client.LocalDomain = "some.domain.com";
                await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.None).ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
            throw new System.NotImplementedException();*/
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(address:_emailConfig.From, name:"Sitio Web de Trueques"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }
        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}