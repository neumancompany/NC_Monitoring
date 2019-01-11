using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_MonitorStatusType")]
    public partial class NcMonitorStatusType : IEntity<int>
    {
        public NcMonitorStatusType()
        {
            NcMonitor = new HashSet<NcMonitor>();
        }

        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty("Status")]
        public virtual ICollection<NcMonitor> NcMonitor { get; set; }
    }
}
