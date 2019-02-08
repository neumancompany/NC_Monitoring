using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NC_Monitoring.ConsoleApp.WebJobs;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.IO;
using NC_Monitoring.Data.Models;
using Microsoft.EntityFrameworkCore;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.ConsoleApp.Classes;
using NC_Monitoring.ConsoleApp.Interfaces;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Repositories;
using NC_Monitoring.Business.Classes;
using System.Net.Mail;
using System.Net;

namespace NC_Monitoring.ConsoleApp
{
    internal class Program
    {
        //private static ILogger<Program> logger;
        private static IServiceProvider serviceProvider;

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = GetWebJobConfiguration();

            AddWebJobsCommonServices(configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(
                            configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>();

            //services.AddAutoMapper();

            services.AddLogging((logging) =>
            {
                logging
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddLog4Net();
            });

            services
                .AddTransient<Monitoring>()
                .AddTransient<IMonitorRecorder, MonitorRecorder>()

                .AddTransient<IMonitorManager, MonitorManager>()
                .AddTransient<IMonitorRepository, MonitorRepository>()
                .AddTransient<IChannelRepository, ChannelRepository>()
                .AddTransient<IRecordRepository, RecordRepository>()
                .AddTransient<IScenarioRepository, ScenarioRepository>();

            services
                .AddTransient<IQueueProvider, QueueProvider>()
                .AddTransient<INotificator, Notificator>()
                .AddTransient<IEmailNotificator, EmailNotificator>();

            services.AddTransient<Functions, Functions>();

            services.AddScoped<SmtpClient>(conf =>
            {
                return new SmtpClient()
                {
                    Host = configuration.GetValue<string>("Email:Smtp:Host"),
                    Port = configuration.GetValue<int>("Email:Smtp:Port"),
                    Credentials = new NetworkCredential(
                        configuration.GetValue<string>("Email:Smtp:Username"),
                        configuration.GetValue<string>("Email:Smtp:Password")
                    )
                };
            });
        }

        private static void Main(string[] args)
        {
            // .NET Core sets the source directory as the working directory - so change that:
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            serviceProvider = services.BuildServiceProvider();

            CultureInfo culture = CultureInfo.CreateSpecificCulture("cs-CZ");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            //Program program = new Program(args);
            var config = new JobHostConfiguration();
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(10);
            config.Queues.VisibilityTimeout = TimeSpan.FromMinutes(1);
            config.Queues.BatchSize = 1;
            config.JobActivator = new JobActivator(serviceProvider);
            config.UseTimers();

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        private static void AddWebJobsCommonServices(IConfigurationRoot configuration)
        {
            if (String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AzureWebJobsStorage")))
            {
                // Env variables would be set on azure. But not locally. If missing, set them to the connection string
                Environment.SetEnvironmentVariable("AzureWebJobsStorage", configuration.GetConnectionString("AzureWebJobsStorage"));
                Environment.SetEnvironmentVariable("AzureWebJobsDashboard", configuration.GetConnectionString("AzureWebJobsDashboard"));
            }
        }

        private static IConfigurationRoot GetWebJobConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}