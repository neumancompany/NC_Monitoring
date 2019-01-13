using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Repositories
{
    public class ChannelRepository : BaseRepository<NcChannel, int>, IChannelRepository
    {
        public ChannelRepository(ApplicationDbContext context) : base(context)
        {
        }

        public NcChannelSubscriber FindSubscriberById(int subscriberId)
        {
            return context.NcChannelSubscriber.Find(subscriberId);
        }

        public List<NcChannelType> GetChannelTypes()
        {
            return context.Set<NcChannelType>().ToList();
        }

        public List<NcChannelSubscriber> GetSubscribersByChannel(int channelId)
        {
            return context.NcChannelSubscriber.Where(x => x.ChannelId == channelId).ToList();
        }

        public List<ApplicationUser> GetUsersNotAssignedToTheChannelYet(int channelId)
        {
            return context.Users
                        .Where(user => !context.NcChannelSubscriber
                            .Where(sub => sub.ChannelId == channelId)
                            .Select(sub => sub.UserId)
                            .Contains(user.Id))
                        .ToList();
        }

        public async Task SubscriberDeleteAsync(int id)
        {
            var entity = await context.NcChannelSubscriber.FindAsync(id);

            context.NcChannelSubscriber.Remove(entity);

            await context.SaveChangesAsync();
        }

        public async Task SubscriberInsertAsync(NcChannelSubscriber subscriber)
        {
            context.NcChannelSubscriber.Add(subscriber);

            await context.SaveChangesAsync();
        }

        public async Task SubscriberUpdateAsync(NcChannelSubscriber subscriber)
        {
            if (context.NcChannelSubscriber.Contains(subscriber))
            {
                context.NcChannelSubscriber.Update(subscriber);
            }
            else
            {
                context.NcChannelSubscriber.Add(subscriber);
            }

            await context.SaveChangesAsync();
        }
    }
}