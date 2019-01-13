using NC_Monitoring.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Interfaces
{
    public interface IChannelRepository : IRepository<NcChannel, int>
    {
        List<NcChannelType> GetChannelTypes();

        List<NcChannelSubscriber> GetSubscribersByChannel(int channelId);

        List<ApplicationUser> GetUsersNotAssignedToTheChannelYet(int channelId);

        NcChannelSubscriber FindSubscriberById(int subscriberId);

        Task SubscriberInsertAsync(NcChannelSubscriber subscriber);

        Task SubscriberUpdateAsync(NcChannelSubscriber subscriber);

        Task SubscriberDeleteAsync(int id);
    }
}