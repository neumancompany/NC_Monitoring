using NC_Monitoring.Business.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.ViewModels
{
    public class ScenarioDetailViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<MonitorListDTO> Monitors { get; set; }
    }
}
