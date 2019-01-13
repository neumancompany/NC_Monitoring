using NC_Monitoring.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Interfaces
{
    public interface IScenarioRepository : IRepository<NcScenario, int>
    {
        IQueryable<NcScenarioItem> GetItems(int scenarioId);

        NcScenarioItem FindItemById(int key);

        Task InsertItemAsync(NcScenarioItem item);

        Task UpdateItemAsync(NcScenarioItem item);

        Task DeleteItemAsync(int key);
        /// <summary>
        /// Vrati vsechny monitory prirazeny ke scenari s <paramref name="scenarioId"/>
        /// </summary>
        /// <param name="scenarioId"></param>
        /// <returns></returns>
        IQueryable<NcMonitor> GetMonitors(int scenarioId);
        IQueryable<NcScenario> GetScenariosByChannelId(int channelId);
    }
}