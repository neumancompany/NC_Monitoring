using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NC.Extensions;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            context.Entry(entity).State = EntityState.Detached;

            var foundEntity = await dbSet.FindAsync(entity.Id);
            bool change = false;

            if (resetTestCycleIfChangeStatus && foundEntity != null)
            {
                if (foundEntity.StatusId != entity.StatusId)
                {//zmena statusu
                    entity.LastTestCycleInterval = null;
                    change = true;
                }
            }

            var allowValues = new Expression<Func<NcMonitor, object>>[]
            {
                x=>x.StatusId,
                x=>x.MethodTypeId,
                x=>x.Url,
                x=>x.VerificationTypeId,
                x=>x.VerificationValue,
                x=>x.ScenarioId,
                x=>x.LastTestCycleInterval,
            }.Select(x=>x.GetPropertyName());

            foreach (var propInfo in foundEntity.GetType().GetProperties().Where(x => allowValues.Contains(x.Name)))
            {
                object oldValue = propInfo.GetValue(foundEntity);
                object newValue = propInfo.GetValue(entity);

                if (oldValue == null && newValue == null)
                {
                    continue;
                }

                if (!Equals(oldValue, newValue))
                {
                    propInfo.SetValue(foundEntity, newValue);
                    change = true;
                }
            }

            if (change)
            {
                await base.UpdateAsync(foundEntity);
            }
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