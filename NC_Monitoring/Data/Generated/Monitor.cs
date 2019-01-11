using System;
using System.Collections.Generic;

namespace NC_Monitoring.Data.Generated
{
    public partial class Monitor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public int MethodTypeId { get; set; }
        public string Url { get; set; }
        public int VerificationTypeId { get; set; }
        public string VerificationValue { get; set; }
        public int Timeout { get; set; }
        public int ScenarioId { get; set; }
    }
}
