﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.ConsoleApp.Interfaces;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.Classes
{
    public class Monitoring
    {
        private readonly ILogger<Monitoring> logger;
        private readonly IMonitorRecorder monitorRecorder;
        private readonly IMonitorManager monitorManager;
        private readonly INotificator notificator;
        private readonly IEmailNotificator emailNotificator;

        public Monitoring(ILogger<Monitoring> logger, IMonitorRecorder monitorRecorder, IMonitorManager monitorManager, INotificator notificator, IEmailNotificator emailNotificator)
        {
            this.logger = logger;
            this.monitorRecorder = monitorRecorder;
            this.monitorManager = monitorManager;
            this.notificator = notificator;
            this.emailNotificator = emailNotificator;
        }

        public Task CheckMonitors(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    List<NcMonitor> monitorsToCheck = monitorManager.MonitorsToCheck();

                    foreach (NcMonitor monitor in monitorsToCheck)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        try
                        {
                            Task task = Task.Run(async () =>
                            {//kontroly jednotlivych monitoru spustime paralelne
                                try
                                {
                                    //vytvoreni noveho scope, jelikoz kazde vlakno by melo mit vlastni scope
                                    //muze se totiz stat, ze se napr. databazovi context uvolni
                                    //a vlakno se na nej bude chtit stale dotazovat
                                    using (IServiceScope scope = serviceProvider.CreateScope())
                                    {
                                        cancellationToken.ThrowIfCancellationRequested();

                                        Monitoring monitoring = scope.ServiceProvider.GetService<Monitoring>();

                                        //vyhledani monitoru v aktualnim scope (db contextu)
                                        NcMonitor tmpMonitor = monitoring.monitorManager.FindMonitor(monitor.Id);
                                        logger.LogInformation($"{tmpMonitor.Name}: Started monitor checking.");
                                        await monitoring.CheckAndRecordMonitorAsync(tmpMonitor);
                                        logger.LogInformation($"{tmpMonitor.Name}: Finished monitor checking.");

                                        cancellationToken.ThrowIfCancellationRequested();

                                        logger.LogInformation($"{tmpMonitor.Name}: Started send all notifications...");

                                        bool saved = false;
                                        while (!saved)
                                        {
                                            try
                                            {
                                                await monitoring.notificator.SendAllNotifications();
                                                saved = true;
                                            }
                                            catch (DbUpdateConcurrencyException ex) {}
                                        }
                                        logger.LogInformation($"{tmpMonitor.Name}: Finished send all notifications...");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex, $"Error while checking monitor: {monitor.Id} - {monitor.Name}.");
                                    await emailNotificator.SendEmailExceptionAsync(ex, $"Error while checking monitor: {monitor.Id} - {monitor.Name}.");
                                }

                            }, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Error while creating and starting task to check monitor: {monitor.Id} - {monitor.Name}.");
                            emailNotificator.SendEmailExceptionAsync(ex, $"Error while creating and starting task to check monitor: {monitor.Id} - {monitor.Name}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{nameof(CheckMonitors)}");
                    emailNotificator.SendEmailExceptionAsync(ex, $"{nameof(CheckMonitors)}");
                }

            }, cancellationToken);
        }

        private Task CheckAndRecordMonitorAsync(NcMonitor monitor)
        {
            var result = CheckMonitor(monitor);

            return monitorRecorder.RecordAsync(monitor, result);
        }

        private MonitorResult CheckMonitor(NcMonitor monitor)
        {
            if (monitor.Enabled)
            {
                int timeout = (int)monitor.Timeout.TotalMilliseconds;
                switch (monitor.VerificationEnum())
                {
                    case MonitorVerification.Keyword:
                        return FindKeywordOnWebsite(monitor.Url, monitor.VerificationValue, timeout);
                    case MonitorVerification.StatusCode:
                        return CheckStatusCode(monitor.Url, monitor.VerificationValue, timeout);
                }

                return MonitorResult.Failed($"{nameof(monitor.VerificationType)} '{monitor.VerificationTypeId} - {monitor.VerificationType}' not found.");
            }

            return MonitorResult.Success;
        }

        private MonitorResult FindKeywordOnWebsite(string url, string keyword, int timeout)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    throw new MonitoringException($"{nameof(keyword)} cant be empty.");
                }

                if (DownloadContent(url, timeout).Contains(keyword))
                {
                    return MonitorResult.Success;
                }

                return MonitorResult.Failed($"Keyword '{keyword}' not found on url '{url}'.");
            }
            catch (Exception ex)
            {
                return MonitorResult.Failed($"Error on checking keyword '{keyword}' on url '{url}'.", ex);
            }
        }

        private MonitorResult CheckStatusCode(string url, string statusCodeNum, int timeout)
        {
            try
            {
                if (int.TryParse(statusCodeNum, out int statusInt))
                {
                    HttpStatusCode statusCode = GetStatusCode(url, timeout);
                    if ((int)statusCode == statusInt)
                    {
                        return MonitorResult.Success;
                    }
                    else
                    {
                        return MonitorResult.Failed($"Url '{url}' response with status code '{(int)statusCode} - {statusCode}', but required '{statusCodeNum}'.");
                    }
                }

                return MonitorResult.Failed($"Status code '{statusCodeNum}' is not valid.");
            }
            catch (Exception ex)
            {
                if (ex is WebException)
                {
                    switch ((ex as WebException).Status)
                    {
                        case WebExceptionStatus.ConnectFailure:
                            return MonitorResult.Failed("Connection failed.", ex);
                        case WebExceptionStatus.NameResolutionFailure:
                            return MonitorResult.Failed("Name reosulution failed.", ex);

                        case WebExceptionStatus.Timeout:
                            return MonitorResult.Failed("Timeout expiration.", ex);

                        case WebExceptionStatus.ProtocolError:
                            if (ex.Message == "The remote server returned an error: (401) unauthorized.")
                            {
                                return MonitorResult.Failed("Anauthorized content (401).", ex);
                            }
                            return MonitorResult.Failed("Protocol error.", ex);
                        default:
                            return MonitorResult.Failed("General WebException error.", ex);
                    }
                }

                return MonitorResult.Failed(ex);
            }
        }

        private HttpStatusCode GetStatusCode(string url, int timeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.Timeout = timeout;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                return response.StatusCode;
            }
        }

        private string DownloadContent(string url, int timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Timeout = timeout;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;//pro https

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        StreamReader readStream = null;

                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream, Encoding.UTF8);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        return readStream.ReadToEnd();
                    }
                }
            }

            return "";
        }
    }
}
