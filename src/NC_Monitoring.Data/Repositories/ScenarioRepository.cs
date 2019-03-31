using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;

namespace NC_Monitoring.Data.Repositories
{
    public class ScenarioRepository : BaseRepository<NcScenario, int>, IScenarioRepository
    {

        private DbSet<NcScenarioItem> entityItems
        {
            get
            {
                return context.NcScenarioItem;
            }
        }

        public ScenarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task DeleteAsync(int id)
        {
            var items = context.NcScenarioItem.Where(x => x.ScenarioId == id);
            context.RemoveRange(items);
            await base.DeleteAsync(id);
        }


        public async Task DeleteItemAsync(int id)
        {
            var item = await entityItems.FindAsync(id);

            if (item != null)
            {
                entityItems.Remove(item);
                await context.SaveChangesAsync();
            }
        }

        public IQueryable<NcScenarioItem> GetItems(int scenarioId)
        {
            return entityItems.Where(x => x.ScenarioId == scenarioId);
        }

        public async Task InsertItemAsync(NcScenarioItem entity)
        {
            entityItems.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(NcScenarioItem entity)
        {
            if (entityItems.Contains(entity))
            {
                entityItems.Update(entity);
            }
            else
            {
                entityItems.Add(entity);
            }

            await context.SaveChangesAsync();
        }

        public NcScenarioItem FindItemById(int key)
        {
            return entityItems.Find(key);
        }
        public IQueryable<NcMonitor> GetMonitors(int scenarioId)
        {
            return context.NcMonitor.Where(x => x.ScenarioId == scenarioId);
        }
        public IQueryable<NcScenario> GetScenariosByChannelId(int channelId)
        {
            return from s in context.NcScenario
                   join si in context.NcScenarioItem on s.Id equals si.ScenarioId
                   where si.ChannelId == channelId
                   select s;
        }
    }
}