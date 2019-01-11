using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_ChannelSubscriber")]
    public partial class NcChannelSubscriber : IEntity<int>
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("ChannelID")]
        public int ChannelId { get; set; }
        
        [ForeignKey("ChannelId")]
        [InverseProperty("NcChannelSubscriber")]
        public virtual NcChannel Channel { get; set; }

        [Column("UserID")]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("NcChannelSubscriber")]
        public virtual ApplicationUser User { get; set; }


    }
}
