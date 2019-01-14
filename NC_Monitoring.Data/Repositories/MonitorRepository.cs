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

        public async Task UpdateAsyncWithoutResetTestCycleInterval(NcMonitor entity)
        {
            await UpdateInternalAsync(entity, resetTestCycleIfChangeStatus: false);
        }

        public override async Task UpdateAsync(NcMonitor entity)
        {
            await UpdateInternalAsync(entity, resetTestCycleIfChangeStatus: true);
        }

        private async Task UpdateInternalAsync(NcMonitor entity, bool resetTestCycleIfChangeStatus)
        {
            var foundEntity = await dbSet.FindAsync(entity.Id);

            if (resetTestCycleIfChangeStatus && foundEntity != null)
            {
                if (foundEntity.StatusId != entity.StatusId)
                {//zmena statusu
                    entity.LastTestCycleInterval = null;
                }
            }

            //mapper.Map(entity, foundEntity);

            foundEntity.MethodTypeId = entity.MethodTypeId;
            foundEntity.StatusId = entity.StatusId;
            foundEntity.ScenarioId = entity.ScenarioId;
            foundEntity.VerificationTypeId = entity.VerificationTypeId;
            foundEntity.VerificationValue = entity.VerificationValue;
            foundEntity.Name = entity.Name;
            foundEntity.Timeout = entity.Timeout;
            foundEntity.Url = entity.Url;
            foundEntity.LastTestCycleInterval = entity.LastTestCycleInterval;

            await base.UpdateAsync(foundEntity);
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