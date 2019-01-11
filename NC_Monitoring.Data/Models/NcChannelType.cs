using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_ChannelType")]
    public partial class NcChannelType : IEntity<int>
    {
        public NcChannelType()
        {
            NcChannel = new HashSet<NcChannel>();
        }

        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty("ChannelType")]
        public virtual ICollection<NcChannel> NcChannel { get; set; }
    }
}
