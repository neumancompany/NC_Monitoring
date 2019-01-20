using NC_Monitoring.Business.DTO;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.Interfaces
{
    public interface IChannelManager
    {

        NcChannel FindById(int channelId);

        /// <summary>
        /// Vrati vsechny uzivatele aplikace, kteri jsou prirazeni k tomuto kanalu
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        List<ApplicationUser> GetUsersByChannel(NcChannel channel);

        /// <summary>
        /// Vrati typy kanalu
        /// </summary>
        /// <returns></returns>
        List<NcChannelType> GetChannelTypes();

        /// <summary>
        /// Vrati vsechny odberale, kteri jsou prirazeni k tomuto kanalu
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        IEnumerable<NcChannelSubscriber> GetSubscribersByChannel(int channelId);

        /// <summary>
        /// Vrati vsechny uzivatele, kteri nejsou prirazeni k tomuto kanalu.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        List<ApplicationUser> GetUsersNotAssignedToChannelYet(int channelId);

        Task SubscriberInsertAsync(NcChannelSubscriber entity);
        NcChannelSubscriber FindSubscriberById(int key);
        Task SubscriberUpdateAsync(NcChannelSubscriber entity);
        Task SubscriberDeleteAsync(int key);
        IQueryable<NcScenario> GetScenariosByChannelId(int id);
    }
}
