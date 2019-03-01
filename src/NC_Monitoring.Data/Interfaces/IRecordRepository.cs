using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Interfaces
{
    public interface IRecordRepository : IRepository<NcMonitorRecord, int>
    {
        Task DeleteAsync(Expression<Func<NcMonitorRecord, bool>> expression);
        Task DeleteAsync();

    }
}