using ExpectedObjects;
using Moq;
using NC.Extensions;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NC_Monitoring.Test.Business
{
    public class ChannelManagerTest
    {        
        [Fact]
        public void FindById_ReturnCorrectResult()
        {
            // Arrange
            int id = 0;
            var expected = new NcChannel
            {
                Id = id,
            };

            var channelRepo = new Mock<IChannelRepository>();
            channelRepo.Setup(x => x.FindById(id)).Returns(expected);

            IChannelManager channelManager = new ChannelManager(
                channelRepo.Object,
                It.IsAny<IScenarioRepository>()
            );

            // Act
            var actual = channelManager.FindById(id);

            // Assert
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(2, 7)]
        public void GetUsersByChannel_ReturnCorrectResult(int channelIndex, int expectedCount)
        {
            // Arrange
            var channels = new NcChannel[]
            {
                GenerateChannelWithSubscribers(1, 2),
                GenerateChannelWithSubscribers(2, 4),
                GenerateChannelWithSubscribers(3, 7),
                GenerateChannelWithSubscribers(4, 1),
            };
            var testChannel = channels[channelIndex];

            var channelRepo = new Mock<IChannelRepository>();
            channelRepo.Setup(x => x.FindById(testChannel.Id)).Returns(testChannel);
            channelRepo.Setup(x => x.GetSubscribersByChannel(testChannel.Id))
                .Returns(testChannel.NcChannelSubscriber.ToList());

            IChannelManager channelManager = new ChannelManager(
                channelRepo.Object,
                It.IsAny<IScenarioRepository>()
            );
            var expected = testChannel.NcChannelSubscriber
                .Select(x=>x.User)
                .ToList();

            // udelat klon hledaneho kanalu, 
            // aby nemohla testovana metoda ovlivnit promennou expected
            var findingChannel = testChannel.Clone();

            // Act
            var actual = channelManager.GetUsersByChannel(findingChannel);

            // Assert
            Assert.Equal(expectedCount, actual.Count);
            expected.ToExpectedObject().ShouldEqual(actual);            
        }

        private NcChannel GenerateChannelWithSubscribers(int channelId, int countSubscribers)
        {
            var result = new NcChannel { Id = channelId };
            var subscribers = new NcChannelSubscriber[countSubscribers];

            for (int i = 0; i < subscribers.Length; i++)
            {
                var userId = Guid.NewGuid();
                subscribers[i] = new NcChannelSubscriber
                {
                    Id = channelId,
                    //Channel = result,
                    UserId = userId,
                    User = new ApplicationUser
                    {
                        Id = userId,
                    }
                };
            }

            result.NcChannelSubscriber = subscribers;

            return result;
        }
    }
}
