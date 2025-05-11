using eCommerce.Shared.Helpers;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            try
            {
                var apiKey = ConfigurationsHelper.SendGrid_APIKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(ConfigurationsHelper.SendGrid_FromEmailAddress, ConfigurationsHelper.SendGrid_FromEmailAddressName);
                var subject = message.Subject;
                var to = new EmailAddress(message.Destination);
                var plainTextContent = message.Body;
                var htmlContent = message.Body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                return client.SendEmailAsync(msg);
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }

        public Task SendToEmailAsync(string fromEmailAddressName, string fromEmailAddress, string toEmailAddress, string toEmailSubject, string toEmailBody)
        {
            try
            {
                var apiKey = ConfigurationsHelper.SendGrid_APIKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmailAddress, fromEmailAddressName);
                var subject = toEmailSubject;
                var to = new EmailAddress(toEmailAddress);
                var plainTextContent = toEmailBody;
                var htmlContent = toEmailBody;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                return client.SendEmailAsync(msg);
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }
    }
}