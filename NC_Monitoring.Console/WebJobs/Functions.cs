using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
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
        //private readonly ILogger<Functions> logger;

        //public Functions(ILogger<Functions> logger)
        //{
        //    this.logger = logger;
        //}

        public void ProcessQueueMessage([TimerTrigger("%MonitoringInterval%"/*, RunOnStartup = true*/)]TimerInfo timerInfo, TextWriter log)
        {
            Console.WriteLine(DateTime.Now);
            //logger.LogInformation(DateTime.Now.ToString());
        }
    }
}
