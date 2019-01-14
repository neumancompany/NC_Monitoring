using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.Classes
{
    public class MonitorRecorder
    {
        private readonly ILogger<MonitorRecorder> logger;
        private readonly IMonitorManager monitorManager;

        public MonitorRecorder(ILogger<MonitorRecorder> logger, IMonitorManager monitorManager)
        {
            this.logger = logger;
            this.monitorManager = monitorManager;
        }

        public async Task RecordAsync(NcMonitor monitor, MonitorResult result)
        {
            var record = monitorManager.LastOpenedMonitorRecord(monitor.Id);

            if (result.IsSuccess)
            {
                if ((record != null && record.EndDate == null) || monitor.StatusEnum() != MonitorStatus.OK)
                {//ukonceni zaznamu
                    if (record != null)
                    {
                        await monitorManager.UpdateEndDateAsync(record, DateTime.Now);
                    }
                    
                    await monitorManager.SetStatusAndResetLastTestCycleIntervalAsync(monitor, MonitorStatus.OK);
                }
            }
            else
            {//nastala chyba
                if (record == null || record.EndDate != null || monitor.StatusEnum() != MonitorStatus.Error)
                {//neexistuje zadny zaznam nebo jsou vsechny uzavreny
                    await monitorManager.SetStatusAndResetLastTestCycleIntervalAsync(monitor, MonitorStatus.Error);
                    await AddNewRecordAsync(monitor, result);
                }
            }
        }

        private async Task AddNewRecordAsync(NcMonitor monitor, MonitorResult result)
        {
            await monitorManager.AddNewRecordAsync(new NcMonitorRecord
            {
                StartDate = DateTime.Now,
                Monitor = monitor,
                Note = GetLogMessage(result),
            });
        }

        private string GetLogMessage(MonitorResult result)
        {
            if (result.Exception != null)
            {//nastala neocekavana chyba
                logger.LogError(result.Exception, result.Message);
            }

            if (string.IsNullOrWhiteSpace(result.Message))
            {
                if (result.Exception != null)
                {
                    return "Unexpected error of exception " + result.Exception.GetType();
                }
                else
                {
                    return "General error";
                }
            }
            else
            {
                return result.Message;
            }
        }
    }
}
