using NC.Utils;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.Managers
{
    public class MonitorManager : IMonitorManager
    {
        
        private readonly IChannelRepository channelRepository;
        private readonly IMonitorRepository monitorRepository;
        private readonly IScenarioRepository scenarioRepository;

        public MonitorManager(IChannelRepository channelRepository, IMonitorRepository monitorRepository, IScenarioRepository scenarioRepository)
        {
            this.channelRepository = channelRepository;
            this.monitorRepository = monitorRepository;
            this.scenarioRepository = scenarioRepository;
        }

        public NcMonitor FindMonitor(Guid id)
        {
            return monitorRepository.FindById(id);
        }
    }
}
