using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ViewModels
{
    public class MonitorFormViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string StatusName { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Display(Name = "Method")]
        [Required]
        public int MethodTypeId { get; set; }

        [Required]
        [Url]
        [StringLength(250)]
        public string Url { get; set; }

        [Display(Name = "Verification type")]
        [Required]
        public int VerificationTypeId { get; set; }

        [Display(Name = "Verification value")]
        [Required]
        [StringLength(250)]
        public string VerificationValue { get; set; }

        [Display(Description = "Format: HH:mm:ss")]
        [Required]
        public TimeSpan Timeout { get; set; }

        [Display(Name = "Scenario")]
        [Required]
        public int ScenarioId { get; set; }
    }
}
