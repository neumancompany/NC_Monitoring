using NC.Utils;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.Managers
{
    public class MonitorManager : IMonitorManager
    {
        /// <summary>
        /// Znaci od kolikateho dne je bran zaznam jako stary.
        /// </summary>
        private const int OLD_RECORD_DAYS = 31;

        private readonly IChannelRepository channelRepository;
        private readonly IMonitorRepository monitorRepository;
        private readonly IRecordRepository recordRepository;
        private readonly IScenarioRepository scenarioRepository;

        public MonitorManager(IChannelRepository channelRepository, IMonitorRepository monitorRepository, IRecordRepository recordRepository, IScenarioRepository scenarioRepository)
        {
            this.channelRepository = channelRepository;
            this.monitorRepository = monitorRepository;
            this.recordRepository = recordRepository;
            this.scenarioRepository = scenarioRepository;
        }

        private List<ApplicationUser> GetUsersByChannel(int channelId)
        {
            return channelRepository.GetSubscribersByChannel(channelId)
                        .Select(x => x.User).ToList();
        }


        public NcMonitor FindMonitor(Guid id)
        {
            return monitorRepository.FindById(id);
        }

        public List<NcMonitor> MonitorsToCheck()
        {
            return monitorRepository.MonitorsToCheck();
        }

        public async Task SetStatusAndResetLastTestCycleIntervalAsync(NcMonitor monitor, MonitorStatus status)
        {
            if (monitor.StatusEnum() != MonitorStatus.InActive)
            {
                monitor.StatusId = (int)status;
            }

            monitor.LastTestCycleInterval = null;
            await monitorRepository.UpdateAsync(monitor);
        }

        public List<NcChannel> GetChannelsToLastCycleTest(NcMonitor monitor)
        {
            return scenarioRepository
                .GetItems(monitor.ScenarioId)
                .Where(x => x.TestCycleInterval <= monitor.LastTestCycleInterval)
                .Select(x=>x.Channel).ToList();
        }

        public List<NcChannel> GetChannelsBetweenLastErrorAndTestCycle(NcMonitor monitor, TimeSpan testCycle)
        {
            if (testCycle.TotalHours >= 24)
            {
                testCycle = new TimeSpan(23, 59, 59);
            }
            return scenarioRepository
                .GetItems(monitor.ScenarioId)
                .Where(x =>
                    (monitor.LastTestCycleInterval == null //monitor jeste nebyl testovan
                        || monitor.LastTestCycleInterval < x.TestCycleInterval )//nebo posledni testovaci cyklus monitoru je mensi nez testovaci cyklus aktualniho kanalu
                    && x.TestCycleInterval <= testCycle)//a zaroven testovaci cyklus kanalu musi byt mensi nebo roven testovacimu cyklu ve vstupnim parametru
                .Select(x => x.Channel).ToList();
        }

        public async Task SetLastTestCycleIntervalsAsync(NcMonitor monitor, TimeSpan? timeSpan)
        {
            monitor.LastTestCycleInterval = timeSpan;
            await monitorRepository.UpdateAsyncWithoutResetTestCycleInterval(monitor);
        }

        #region "Records"

        public TimeSpan? TimeInErrorStatus(Guid monitorId)
        {
            NcMonitorRecord record = LastOpenedMonitorRecord(monitorId);

            if (record == null || record.EndDate != null) return null;

            return DateTime.Now - record.StartDate;
        }

        public IQueryable<NcMonitorRecord> GetRecordsForMonitor(Guid monitorId)
        {
            return recordRepository.GetAll().Where(x => x.MonitorId == monitorId);
        }

        public IQueryable<NcMonitorRecord> GetAllRecords()
        {
            return recordRepository.GetAll();
        }

        public NcMonitorRecord LastOpenedMonitorRecord(Guid monitorId)
        {
            return recordRepository.GetAll()
                        .Where(x => x.MonitorId == monitorId && x.EndDate == null)
                        .OrderByDescending(x => x.StartDate)
                        .FirstOrDefault();
        }

        public async Task AddNewRecordAsync(NcMonitorRecord record)
        {
            await recordRepository.InsertAsync(record);
        }

        public async Task UpdateEndDateAsync(NcMonitorRecord record, DateTime endDate)
        {
            record.EndDate = endDate;
            await recordRepository.UpdateAsync(record);
        }

        public async Task DeleteRecordsForMonitorAsync(Guid monitorid)
        {
            await recordRepository.DeleteAsync(x => x.MonitorId == monitorid);
        }

        public async Task DeleteOldRecordsForMonitorAsync(Guid monitorId)
        {
            await recordRepository.DeleteAsync(x => x.MonitorId==monitorId && x.StartDate < DateTime.Now.AddDays(-OLD_RECORD_DAYS) && x.EndDate != null);
        }

        public async Task DeleteOldRecordsAsync()
        {
            await recordRepository.DeleteAsync(x=>x.StartDate < DateTime.Now.AddDays(-OLD_RECORD_DAYS) && x.EndDate != null);
        }
        public async Task DeleteRecordsAsync()
        {
            await recordRepository.DeleteAsync();
        }
        #endregion
    }
}
