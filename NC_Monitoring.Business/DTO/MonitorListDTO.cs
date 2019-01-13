using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.DTO
{
    public class MonitorListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ScenarioId { get; set; }
        public string StatusName { get; set; }
        public bool Enabled { get; set; }
        public int MethodTypeId { get; set; }
        public string Url { get; set; }
        public int VerificationTypeId { get; set; }
        public string VerificationValue { get; set; }
        public int Timeout { get; set; }
    }
}
