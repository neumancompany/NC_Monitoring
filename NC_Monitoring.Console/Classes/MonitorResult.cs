using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.Classes
{
    public class MonitorResult
    {
        public static MonitorResult Success
        {
            get
            {
                return new MonitorResult
                {
                    IsSuccess = true,
                    Exception = null,
                };
            }
        }

        public static MonitorResult Failed(string message)
        {
            return Failed(message, null);
        }

        public static MonitorResult Failed(Exception ex)
        {
            return Failed(string.Empty, ex);
        }

        public static MonitorResult Failed(string message, Exception ex)
        {
            return new MonitorResult
            {
                IsSuccess = false,
                Message = message,
                Exception = ex,
            };
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
        public Exception Exception { get; set; }        
    }
}
