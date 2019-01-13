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
        
    }
}
