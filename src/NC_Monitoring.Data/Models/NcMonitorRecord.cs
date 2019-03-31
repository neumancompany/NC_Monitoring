using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_MonitorRecord")]
    public partial class NcMonitorRecord : IEntity<int>
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("MonitorID")]
        public Guid MonitorId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        public string Note { get; set; }

        [ForeignKey("MonitorId")]
        [InverseProperty("NcMonitorRecord")]
        public virtual NcMonitor Monitor { get; set; }
    }
}
