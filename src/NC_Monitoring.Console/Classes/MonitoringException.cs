using NC_Monitoring.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.Classes
{
    public class MonitoringException : Exception
    {
        public MonitoringException(string message) : base(message) { }

        public MonitoringException(string message, Exception innerException) : base(message, innerException) { }
    }
}
