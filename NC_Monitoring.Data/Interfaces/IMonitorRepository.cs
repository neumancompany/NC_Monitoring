using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Interfaces
{
    public interface IMonitorRepository : IRepository<NcMonitor, Guid>
    {
        List<NcMonitorStatusType> GetStatusTypes();
        List<NcMonitorMethodType> GetMethodTypes();
        List<NcMonitorVerificationType> GetVerificationTypes();

        List<NcMonitor> MonitorsToCheck();

        /// <summary>
        /// Aktulizuje monitor bez restartu intervalu testovaciho cyklu.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsyncWithoutResetTestCycleInterval(NcMonitor entity);
    }
}