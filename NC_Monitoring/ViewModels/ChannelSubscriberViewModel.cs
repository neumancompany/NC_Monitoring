using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ViewModels
{
    public class ChannelSubscriberViewModel
    {
        public int Id { get; set; }
        [Display(Name = "User")]
        public Guid UserId { get; set; }

    }
}
