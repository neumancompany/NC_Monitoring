using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.ConsoleApp.Classes;
using NC_Monitoring.ConsoleApp.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.WindowsService
{
    public class MonitoringService : BackgroundService
    {
        private readonly Monitoring monitoring;
        private readonly INotificator notificator;
        private readonly IEmailNotificator emailNotificator;
        private readonly ILogger<MonitoringService> logger;
        private readonly IServiceProvider services;
        private readonly IConfiguration configuration;

        private int MonitoringIntervalMilliseconds
        {
            get
            {
                int tmp = configuration.GetSection("MonitoringIntervalSeconds").Get<int>();
                if (tmp <= 0)
                {
                    return 60000; // minuta
                }

                return tmp * 1000;
            }
        }

        public MonitoringService(Monitoring monitoring, INotificator notificator,
            IEmailNotificator emailNotificator, ILogger<MonitoringService> logger, IServiceProvider services,
            IConfiguration configuration)
        {
            this.monitoring = monitoring;
            this.notificator = notificator;
            this.emailNotificator = emailNotificator;
            this.logger = logger;
            this.services = services;
            this.configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[{nameof(MonitoringService)}] has been started.....");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    monitoring.CheckMonitors(services, cancellationToken);

                    //Thread.Sleep(MonitoringIntervalMilliseconds); - pote nefunguje preruseni vlakna v konzoli
                    //bool canceled = cancellationToken.WaitHandle.WaitOne(MonitoringIntervalMilliseconds);

                    //var workItem = await TaskQueue.DequeueAsync(cancellationToken);

                    //try
                    //{
                    //    await workItem(cancellationToken);
                    //}
                    //catch (Exception ex)
                    //{
                    //    logger.LogError(ex,
                    //       $"Error occurred executing {nameof(workItem)}.");
                    //}
                    await Task.Delay(MonitoringIntervalMilliseconds, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await emailNotificator.SendEmailExceptionAsync(ex, "Console application to monitoring websites is down.");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"[{nameof(MonitoringService)}] has been stopped.....");
            await emailNotificator.SendEmailErrorAsync("Console application to monitoring websites is stopped.");
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
