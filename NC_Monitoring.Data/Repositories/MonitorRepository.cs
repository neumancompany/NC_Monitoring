using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Repositories
{
    public class MonitorRepository : BaseRepository<NcMonitor, Guid>, IMonitorRepository
    {
        private readonly IMapper mapper;

        public MonitorRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }
        
        public List<NcMonitorMethodType> GetMethodTypes()
        {
            return context.NcMonitorMethodType.ToList();
        }

        public List<NcMonitorStatusType> GetStatusTypes()
        {
            return context.NcMonitorStatusType.ToList();
        }

        public List<NcMonitorVerificationType> GetVerificationTypes()
        {
            return context.NcMonitorVerificationType.ToList();
        }

        public List<NcMonitor> MonitorsToCheck()
        {
            return context.NcMonitor.Where(x => x.Enabled).ToList();
        }
    }
}