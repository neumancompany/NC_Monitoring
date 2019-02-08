using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NC_Monitoring.ConsoleApp.Classes;
using NC_Monitoring.ConsoleApp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.WebJobs
{
    public class Functions
    {
        private readonly Monitoring monitoring;
        private readonly INotificator notificator;
        private readonly ILogger<Functions> logger;
        private readonly IServiceProvider serviceProvider;

        public Functions(Monitoring monitoring, INotificator notificator, ILogger<Functions> logger, IServiceProvider serviceProvider)
        {
            this.monitoring = monitoring;
            this.notificator = notificator;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public void ProcessQueueMessage([TimerTrigger("%MonitoringInterval%"/*, RunOnStartup = true*/)]TimerInfo timerInfo, TextWriter log)
        {
            var msg = "Start monitor: " + DateTime.Now;
            logger.LogInformation(msg);
            Console.WriteLine(msg);
            monitoring.CheckMonitors(serviceProvider);
            //await notificator.SendAllNotifications();
            notificator.SendAllNotifications().Wait();
        }
    }
}
