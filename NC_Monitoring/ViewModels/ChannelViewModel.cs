using NC_Monitoring.Business.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NC_Monitoring.ViewModels
{
    public class ChannelViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //[Required]
        [Required]
        [Display(Name = "Notification provider")]
        public int ChannelTypeId { get; set; }

        //public ICollection<NcScenarioItem> NcScenarioItem { get; set; }
    }
}