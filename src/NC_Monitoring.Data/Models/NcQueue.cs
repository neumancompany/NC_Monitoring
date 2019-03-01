using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NC_Monitoring.Data.Models
{
    [Table("NC_Queue")]
    public partial class NcQueue : IEntity<int>
    {
        [Column("ID")]        
        public int Id { get; set; }

        [MaxLength(50)]
        public QueueType Type { get; set; }        
                
        public string Message { get; set; }
    }
}
