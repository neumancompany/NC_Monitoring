using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Extensions
{
    public static class MonitorExtensions
    {
        public static MonitorVerification VerificationEnum(this NcMonitor monitor)
        {
            return (MonitorVerification)monitor.VerificationTypeId;
        }

        public static MonitorStatus StatusEnum(this NcMonitor monitor)
        {
            return (MonitorStatus)monitor.StatusId;
        }
    }
}
