using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.Managers
{
    public class ChannelManager : IChannelManager
    {
        private readonly IChannelRepository channelRepository;
        private readonly IScenarioRepository scenarioRepository;

        public ChannelManager(IChannelRepository channelRepository, IScenarioRepository scenarioRepository)
        {
            this.channelRepository = channelRepository;
            this.scenarioRepository = scenarioRepository;
        }

        public List<NcChannelType> GetChannelTypes()
        {
            return channelRepository.GetChannelTypes();
        }

        public List<ApplicationUser> GetUsersByChannel(NcChannel channel)
        {
            var users = new List<ApplicationUser>();
            foreach (var subscribers in channelRepository.FindById(channel.Id).NcChannelSubscriber)
            {
                users.Add(subscribers.User);
            }
            return users;
        }
        public IEnumerable<NcChannelSubscriber> GetSubscribersByChannel(int channelId)
        {
            return channelRepository.GetSubscribersByChannel(channelId);
        }
        public async Task SubscriberInsertAsync(NcChannelSubscriber entity)
        {
            await channelRepository.SubscriberInsertAsync(entity);
        }
        public NcChannelSubscriber FindSubscriberById(int key)
        {
            return channelRepository.FindSubscriberById(key);
        }
        public async Task SubscriberUpdateAsync(NcChannelSubscriber entity)
        {
            await channelRepository.SubscriberUpdateAsync(entity);
        }
        public async Task SubscriberDeleteAsync(int key)
        {
            await channelRepository.SubscriberDeleteAsync(key);
        }

        public NcChannel FindById(int channelId)
        {
            return channelRepository.FindById(channelId);
        }

        public List<ApplicationUser> GetUsersNotAssignedToChannelYet(int channelId)
        {
            return channelRepository.GetUsersNotAssignedToTheChannelYet(channelId);
        }
        public IQueryable<NcScenario> GetScenariosByChannelId(int id)
        {
            return scenarioRepository.GetScenariosByChannelId(id);
        }
    }
}
