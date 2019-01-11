using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Models
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public bool GlobalAdmin { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<NcChannelSubscriber> NcChannelSubscriber { get; set; }
            = new HashSet<NcChannelSubscriber>();
    }
}
