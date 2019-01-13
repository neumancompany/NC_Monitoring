using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.DTO
{
    public class ScenarioItemListDTO
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public TimeSpan TestCycleCount { get; set; }
    }
}
