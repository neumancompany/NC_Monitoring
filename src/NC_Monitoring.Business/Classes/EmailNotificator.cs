using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Interfaces;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.Classes
{
    public class EmailNotificator : IEmailNotificator, IDisposable
    {
        private readonly SmtpClient smtpClient;
        private readonly ILogger<EmailNotificator> logger;
        //private readonly IConfiguration configuration;

        private readonly string addressFrom;

        public EmailNotificator(SmtpClient smtpClient, ILogger<EmailNotificator> logger, IConfiguration configuration)
        {
            this.smtpClient = smtpClient;
            this.logger = logger;

            this.addressFrom = configuration["Email:From"];
        }

        public void Dispose()
        {
            smtpClient.Dispose();
        }

        public async Task SendEmailAsync(string emailTo, string subject, string message)
        {
            if (emailTo == null)
            {
                throw new ArgumentNullException($"{nameof(emailTo)} cant be null.");
            }

            using (var mailMsg = new MailMessage() { From = new MailAddress(addressFrom), Subject = subject, Body = message, })
            {
                foreach (string email in emailTo.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mailMsg.To.Add(email);
                }
                await smtpClient.SendMailAsync(mailMsg);
                logger.LogInformation($"SendEmail: To=[{emailTo}] Subject[{subject}] Message=[{message}]");
            }
        }
    }
}
