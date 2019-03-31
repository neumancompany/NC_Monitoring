using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.Interfaces
{
    public interface IMonitorManager
    {

        NcMonitor FindMonitor(Guid id);

        Task SetStatusAndResetLastTestCycleIntervalAsync(NcMonitor monitor, MonitorStatus status);

        //List<ApplicationUser> GetUnnotifiedUsersForTestCycle(NcMonitor monitor, TimeSpan testCycle);

        /// <summary>
        /// Vrati vsechny kanaly, ktere jsou mezi intervaly <paramref name="testCycle"/> (vcetne) a <see cref="NcMonitor.LastTestCycleInterval"/>.
        /// </summary>
        /// <returns></returns>
        List<NcChannel> GetChannelsBetweenLastErrorAndTestCycle(NcMonitor monitor, TimeSpan testCycle);

        /// <summary>
        /// Vrati vsechny kanaly, ktere jsou mensi nebo rovny poslednimu testovacimu cyklu co je na monitoru
        /// </summary>
        /// <param name="monitor"></param>
        /// <returns></returns>
        List<NcChannel> GetChannelsToLastCycleTest(NcMonitor monitor);


        ///// <summary>
        ///// Zmeni monitoru status a vyresetuje hodnotu <see cref="NcMonitor.LastTestWhileError"/>.
        ///// </summary>
        ///// <param name="monitor"></param>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //Task ChangeMonitorStatusAsync(NcMonitor monitor, MonitorStatus status);

        ///// <summary>
        ///// Vrati vsechny uzivatele, kteri jsou priazeni k monitoru <paramref name="monitor"/>.
        ///// Jejich testovaci cyklus musi byt zaroven mensi nebo roven zadanemu parametru <paramref name="testCycle"/>
        ///// a museji spadat pod typ kanalu <paramref name="channelType"/>
        ///// </summary>
        ///// <param name="monitor"></param>
        ///// <param name="testCycle"></param>
        ///// <param name="channelType"></param>
        ///// <returns></returns>
        //List<ApplicationUser> GetUnnotifiedUsersForLowerOrEqualsTestCycle(NcMonitor monitor, TimeSpan testCycle, ChannelType channelType);

        //List<ApplicationUser> GetUsersByChannel(NcChannel channel);

        List<NcMonitor> MonitorsToCheck();

        Task SetLastTestCycleIntervalsAsync(NcMonitor monitor, TimeSpan? timeSpan);

        #region "Records"

        /// <summary>
        /// Vrati cas jak dlouho je monitor v chybovem stavu. Vraci NULL pokud jiz monitor neni v chybovem stavu.
        /// </summary>
        /// <param name="monitorId"></param>
        /// <returns></returns>
        TimeSpan? TimeInErrorStatus(Guid monitorId);

        IQueryable<NcMonitorRecord> GetRecordsForMonitor(Guid monitorId);

        IQueryable<NcMonitorRecord> GetAllRecords();

        /// <summary>
        /// Vrati posledni neuzavreny zaznam. Pokud jsou vsechny uzavreny, tak vraci NULL.
        /// </summary>
        /// <param name="monitorId"></param>
        /// <returns></returns>
        NcMonitorRecord LastOpenedMonitorRecord(Guid monitorId);

        Task AddNewRecordAsync(NcMonitorRecord record);

        Task UpdateEndDateAsync(NcMonitorRecord record, DateTime endDate);

        Task DeleteRecordsForMonitorAsync(Guid monitorId);
        Task DeleteOldRecordsForMonitorAsync(Guid monitorId);

        Task DeleteOldRecordsAsync();
        Task DeleteRecordsAsync();

        #endregion
    }
}
