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

        /// <summary>
        /// Nastavi status monitoru podle parametru <paramref name="status"/>
        /// a restartuje interval testovaciho cyklu.
        /// </summary>
        /// <param name="monitor"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task SetStatusAndResetLastTestCycleIntervalAsync(NcMonitor monitor, MonitorStatus status);
        
        /// <summary>
        /// Vrati vsechny monitory, ktere jsou aktivni
        /// </summary>
        /// <returns></returns>
        List<NcMonitor> MonitorsToCheck();
        
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
