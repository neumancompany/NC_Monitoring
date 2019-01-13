﻿using NC_Monitoring.Data.Models;
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
    }
}