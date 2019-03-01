using NC.AspNetCore.Attributes;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_Monitor")]
    public partial class NcMonitor : IEntity<Guid>
    {
        public NcMonitor()
        {
            NcMonitorRecord = new HashSet<NcMonitorRecord>();
        }

        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("StatusID")]
        public int StatusId { get; set; }

        [Column("MethodTypeID")]
        public int MethodTypeId { get; set; }

        [Required]
        [StringLength(250)]
        public string Url { get; set; }

        [Column("VerificationTypeID")]
        public int VerificationTypeId { get; set; }

        [Required]
        [StringLength(250)]
        public string VerificationValue { get; set; }
        public TimeSpan Timeout { get; set; }

        [Column("ScenarioID")]
        public int ScenarioId { get; set; }

        [ForeignKey("ScenarioId")]
        [InverseProperty("NcMonitor")]
        public virtual NcScenario Scenario { get; set; }

        [ForeignKey("MethodTypeId")]
        [InverseProperty("NcMonitor")]
        public virtual NcMonitorMethodType MethodType { get; set; }

        [ForeignKey("StatusId")]
        [InverseProperty("NcMonitor")]
        public virtual NcMonitorStatusType Status { get; set; }

        [ForeignKey("VerificationTypeId")]
        [InverseProperty("NcMonitor")]
        public virtual NcMonitorVerificationType VerificationType { get; set; }

        [InverseProperty("Monitor")]
        public virtual ICollection<NcMonitorRecord> NcMonitorRecord { get; set; }

        [Column("LastTestCycleInterval")]
        public TimeSpan? LastTestCycleInterval { get; set; }

        public bool Enabled
        {
            get
            {
                return this.StatusEnum() != MonitorStatus.InActive;
            }
        }
    }
}
