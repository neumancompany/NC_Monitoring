using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_Scenario")]
    public partial class NcScenario : IEntity<int>
    {
        public NcScenario()
        {
            NcMonitor = new HashSet<NcMonitor>();
            NcScenarioItem = new HashSet<NcScenarioItem>();
        }

        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty("Scenario")]
        public virtual ICollection<NcMonitor> NcMonitor { get; set; }
        [InverseProperty("Scenario")]
        public virtual ICollection<NcScenarioItem> NcScenarioItem { get; set; }
    }
}
