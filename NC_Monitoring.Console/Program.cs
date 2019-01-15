﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Classes;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.ConsoleApp.Classes;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.Data.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp
{
    internal class Program
    {
        private readonly ILogger<Program> logger;
        private readonly IConfiguration Configuration;
        private readonly IServiceProvider serviceProvider;

        public Program(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(
                            Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>();

            services.AddAutoMapper();

            services.AddLogging(conf => { conf.AddLog4Net(); });

            services
                .AddTransient<Monitoring>()
                .AddTransient<MonitorRecorder>()

                .AddTransient<IMonitorManager, MonitorManager>()
                .AddTransient<IMonitorRepository, MonitorRepository>()
                .AddTransient<IChannelRepository, ChannelRepository>()                
                .AddTransient<IRecordRepository, RecordRepository>()
                .AddTransient<IScenarioRepository, ScenarioRepository>();

            services
                .AddTransient<QueueProvider>()
                .AddTransient<Notificator>()
                .AddTransient<EmailNotificator>();

            services.AddScoped<SmtpClient>(conf =>
            {
                return new SmtpClient()
                {
                    Host = Configuration.GetValue<string>("Email:Smtp:Host"),
                    Port = Configuration.GetValue<int>("Email:Smtp:Port"),
                    Credentials = new NetworkCredential(
                        Configuration.GetValue<string>("Email:Smtp:Username"),
                        Configuration.GetValue<string>("Email:Smtp:Password")
                    )
                };
            });

            serviceProvider = services
                .BuildServiceProvider();

            logger = serviceProvider.GetService<ILoggerFactory>()
                .AddLog4Net()
                .CreateLogger<Program>();
        }

        private static void Main(string[] args)
        {
            Program program = new Program(args);

            program.Run().Wait();
        }

        private async Task Run()
        {
            try
            {
                if (!TimeSpan.TryParse(Configuration["MonitoringInterval"], out TimeSpan interval))
                {
                    interval = TimeSpan.FromMinutes(2);
                }

                while (true)
                {
                    try
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            var serviceProvider = scope.ServiceProvider;

                            serviceProvider
                                .GetService<Monitoring>()
                                .CheckMonitors(serviceProvider);

                            await serviceProvider.GetService<Notificator>().SendAllNotifications();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "CheckMonitors: Unexpected error");
                    }
                    await Task.Delay(interval);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error");
            }
        }
    }
}