using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_ScenarioItem")]
    public partial class NcScenarioItem : IEntity<int>
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("ScenarioID")]
        public int ScenarioId { get; set; }
        [Column("ChannelID")]
        public int ChannelId { get; set; }
        public TimeSpan TestCycleInterval { get; set; }

        [ForeignKey("ChannelId")]
        [InverseProperty("NcScenarioItem")]
        public virtual NcChannel Channel { get; set; }
        [ForeignKey("ScenarioId")]
        [InverseProperty("NcScenarioItem")]
        public virtual NcScenario Scenario { get; set; }
    }
}
