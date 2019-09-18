using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions;
using Microsoft.Extensions.Logging;
using NC_Monitoring.ConsoleApp.Classes;
using NC_Monitoring.ConsoleApp.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using NC_Monitoring.Business.Interfaces;
using Microsoft.Extensions.Configuration;

namespace NC_Monitoring.ConsoleApp.WebJobs
{
    public class Functions
    {
        private string NotificationEmailOnError
        {
            get
            {
                return configuration.GetSection("ConsoleApp:NotificationEmailOnError").Get<string>();
            }
        }

        private readonly Monitoring monitoring;
        private readonly INotificator notificator;
        private readonly IEmailNotificator emailNotificator;
        private readonly ILogger<Functions> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        public Functions(Monitoring monitoring, INotificator notificator, IEmailNotificator emailNotificator, ILogger<Functions> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.monitoring = monitoring;
            this.notificator = notificator;
            this.emailNotificator = emailNotificator;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
        }

        public async Task ProcessQueueMessage([TimerTrigger("%MonitoringInterval%"/*, RunOnStartup = true*/)]TimerInfo timerInfo, TextWriter log)
        {
            try
            {
                var msg = "Start monitor: " + DateTime.Now;

                logger.LogInformation(msg);

                //monitoring.CheckMonitors(serviceProvider);

                await notificator.SendAllNotifications();
            }
            catch (Exception ex)
            {
                string email = NotificationEmailOnError;
                if (email != null)
                {
                    await emailNotificator.SendEmailAsync(email, "!!! MONITORING WEBJOB DOWN !!!", "Console application to monitoring websites is down."
                        + $"{Environment.NewLine}Exception:{Environment.NewLine}" + ex.ToString());
                }
            }

        }
    }
}
