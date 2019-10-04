using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        private string NewLine => Environment.NewLine;

        private readonly string notificationEmailOnError;
        private readonly string addressFrom;

        private readonly string machineName = Environment.MachineName;
        private readonly string userName = Environment.UserName;

        public EmailNotificator(SmtpClient smtpClient, ILogger<EmailNotificator> logger, IConfiguration configuration)
        {
            this.smtpClient = smtpClient;
            this.logger = logger;

            this.addressFrom = configuration["Email:From"];
            notificationEmailOnError = configuration.GetSection("ConsoleApp:NotificationEmailOnError").Get<string>();
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

            using (var mailMsg = new MailMessage() { From = new MailAddress(addressFrom), Subject = $"{machineName}: {subject}", Body = message, })
            {
                foreach (string email in emailTo.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mailMsg.To.Add(email);
                }
                await smtpClient.SendMailAsync(mailMsg);
                logger.LogInformation($"SendEmail: To=[{emailTo}] Subject[{subject}] Message=[{message}]");
            }
        }

        public async Task SendEmailErrorAsync(string message, params object[] data)
        {
            await SendEmailExceptionAsync(null, "!!! MONITORING SERVICE ERROR !!!", message, data);
        }

        public async Task SendEmailExceptionAsync(Exception ex, string message, params object[] data)
        {
            await SendEmailExceptionAsync(ex, "!!! MONITORING SERVICE EXCEPTION !!!", message, data);
        }

        private async Task SendEmailExceptionAsync(Exception ex, string subject, string message, params object[] data)
        {
            string email = notificationEmailOnError;

            if (!string.IsNullOrWhiteSpace(email))
            {
                List<string> emailMessageLines = new List<string>();

                emailMessageLines.Add($"MachineName: {machineName}");
                emailMessageLines.Add($"UserName: {userName}");

                emailMessageLines.Add(message);

                if (ex != null)
                {
                    emailMessageLines.Add("Exception:");
                    emailMessageLines.Add(string.IsNullOrWhiteSpace(ex.StackTrace) ? ex.Message : ex.StackTrace);
                }

                if (data != null && data.Length > 0)
                {
                    emailMessageLines.Add("Data:");
                    emailMessageLines.Add($"{JsonConvert.SerializeObject(data)}");
                }

                await SendEmailAsync(email, subject, string.Join(NewLine + NewLine, emailMessageLines));
            }
        }
    }
}
