using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
            using (var mailMsg = new MailMessage(addressFrom, emailTo) { Subject = subject, Body = message, })
            {
                await smtpClient.SendMailAsync(mailMsg);
                logger.LogInformation($"SendEmail: To=[{emailTo}] Subject[{subject}] Message=[{message}]");
            }
        }
    }
}
