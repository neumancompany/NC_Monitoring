using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ViewModels
{
    public class ScenarioItemViewModel
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ScenarioId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Channel")]
        public int ChannelId { get; set; }

        [Required]
        [Display(Name = "Notification time", Description = "Format: HH:mm:ss")]
        //[Range(typeof(TimeSpan), "00:00:00", "23:59:00")]
        public TimeSpan TestCycleInterval { get; set; }

        //public NcChannel Channel { get; set; }
        //public NcScenario Scenario { get; set; }
    }
}
