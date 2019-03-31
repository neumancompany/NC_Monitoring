using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ViewModels
{
    public class MonitorRecordViewModel
    {
        public string MonitorName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public DateTime GroupBy { get; set; }
    }
}
