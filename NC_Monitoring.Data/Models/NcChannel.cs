using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_Channel")]
    public partial class NcChannel : IEntity<int>
    {
        public NcChannel()
        {
            NcScenarioItem = new HashSet<NcScenarioItem>();
            NcChannelSubscriber = new HashSet<NcChannelSubscriber>();
        }

        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("ChannelTypeID")]
        public int ChannelTypeId { get; set; }

        [ForeignKey("ChannelTypeId")]
        [InverseProperty("NcChannel")]
        public virtual NcChannelType ChannelType { get; set; }
        [InverseProperty("Channel")]
        public virtual ICollection<NcScenarioItem> NcScenarioItem { get; set; }

        [InverseProperty("Channel")]
        public virtual ICollection<NcChannelSubscriber> NcChannelSubscriber { get; set; }
    }
}
