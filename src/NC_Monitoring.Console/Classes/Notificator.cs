using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Classes;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.ConsoleApp.Interfaces;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.Classes
{
    public class Notificator : INotificator
    {
        private readonly IMonitorManager monitorManager;
        private readonly ApplicationUserManager applicationUserManager;
        private readonly IEmailNotificator emailNotificator;

        private readonly IQueueProvider queue;
        private readonly ILogger<Notificator> logger;

        class NotifyItem
        {
            public string Subject { get; set; }
            public string Message { get; set; }
            public ChannelType ChannelType { get; set; }

            public string Contact { get; set; }
        }

        public Notificator(IMonitorManager monitorManager, ApplicationUserManager applicationUserManager, IEmailNotificator emailNotificator, IQueueProvider queue, ILogger<Notificator> logger)
        {
            this.monitorManager = monitorManager;
            this.applicationUserManager = applicationUserManager;
            this.emailNotificator = emailNotificator;
            this.queue = queue;
            this.logger = logger;
        }



        /// <summary>
        /// Prida notifikaci o nahozeni do fronty.
        /// </summary>
        /// <param name="monitor"></param>
        /// <returns></returns>
        public async Task AddNotifyUpAsync(NcMonitor monitor)
        {
            await NotifyAsync(monitor,
                $"[MONITORING UP] {monitor.Name}",
                $"Monitor '{monitor.Name}' for url '{monitor.Url}' with verificaiton rule '{monitor.VerificationEnum()}={monitor.VerificationValue}' is now up.",
                notifyUp: true);
        }

        /// <summary>
        /// Prida notifikaci o vypadku do fronty.
        /// </summary>
        /// <param name="monitor"></param>
        /// <returns></returns>
        public async Task AddNotifyDownAsync(NcMonitor monitor)
        {
            NcMonitorRecord monitorRecord = monitorManager.LastOpenedMonitorRecord(monitor.Id);

            if (monitorRecord == null)
            {//vsechny zaznamy jsou jiz uzavreny, uz neni error.
                return;
            }

            await NotifyAsync(monitor,
                    $"[MONITORING DOWN] {monitor.Name}",
                    $"Monitor '{monitor.Name}' for url '{monitor.Url}' with verificaiton rule '{monitor.VerificationEnum()}={monitor.VerificationValue}' is now down." +
                    $"\n\nRecord note: {monitorRecord.Note}",
                notifyUp: false);
        }

        /// <summary>
        /// Odesle postupne vsechny notifikace co jsou ve fronte.
        /// </summary>
        /// <returns></returns>
        public async Task SendAllNotifications()
        {
            NotifyItem notifyItem = await queue.PopAsync<NotifyItem>(QueueType.Notification);

            while (notifyItem != null)
            {
                switch (notifyItem.ChannelType)
                {
                    case ChannelType.Email:
                        await emailNotificator.SendEmailAsync(notifyItem.Contact, notifyItem.Subject, notifyItem.Message);
                        break;
                    default:
                        logger.LogError($"Notificaiton sending for '{notifyItem.ChannelType}' not implemented yet.");
                        break;
                }

                notifyItem = await queue.PopAsync<NotifyItem>(QueueType.Notification);
            }
        }

        private async Task NotifyAsync(NcMonitor monitor, string subject, string message, bool notifyUp)
        {
            TimeSpan? timeInErrorStatus = monitorManager.TimeInErrorStatus(monitor.Id);

            if (notifyUp && timeInErrorStatus != null       //ma se odeslat upozorneni o nahozeni, ale monitor je stale v chybovem stavu
                || !notifyUp && timeInErrorStatus == null)  //ma se odeslat upozorneni o spadnuti, ale monitor neni v chybovem stavu
            {
                return;
            }

            List<NcChannel> channels;

            if (notifyUp)
            {//oznamit vsem kanalum co jiz byly upozorneni, ze web jiz bezi
                channels = monitorManager.GetChannelsToLastCycleTest(monitor);
            }
            else
            {//oznamit vsem kanalum co jeste nebyly upozorneni a maji byt upozorneni
                channels = monitorManager.GetChannelsBetweenLastErrorAndTestCycle(monitor, timeInErrorStatus ?? TimeSpan.Zero);
            }


            bool foundChannels = channels.Count > 0;

            if (foundChannels)
            {
                foreach (NcChannel channel in channels)
                {
                    ChannelType chnlType = channel.ChannelTypeEnum();
                    Func<NcChannelSubscriber, string> contactSelect;

                    switch (chnlType)
                    {
                        case ChannelType.Email:
                            contactSelect = x => x.User.Email;
                            break;
                        default:
                            logger.LogError($"Method '{nameof(NotifyAsync)}' is not implemented for  '{chnlType.GetType()}.{chnlType}' yet.");
                            continue;
                    }

                    await queue.PushAsync(QueueType.Notification, new NotifyItem
                    {
                        ChannelType = chnlType,
                        Subject = subject,
                        Message = message,
                        Contact = string.Join(";", channel.NcChannelSubscriber.Select(contactSelect)),
                    });
                }

                await monitorManager.SetLastTestCycleIntervalsAsync(monitor, timeInErrorStatus);
            }
        }
    }
}
