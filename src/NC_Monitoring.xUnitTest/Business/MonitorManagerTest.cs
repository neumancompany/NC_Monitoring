using ExpectedObjects;
using Moq;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NC_Monitoring.Test.Business
{
    public class MonitorManagerTest
    {
        [Theory]
        [InlineData(MonitorStatus.Error, MonitorStatus.InActive)]
        [InlineData(MonitorStatus.InActive, MonitorStatus.InActive)]
        public async Task SetStatusAndResetLastTestCycleIntervalAsync(
            MonitorStatus oldStatus,
            MonitorStatus newStatus)
        {
            // Arrange
            var monitor = new NcMonitor
            {
                StatusId = (int)oldStatus,
                LastTestCycleInterval = new TimeSpan(2, 3, 4),
            };

            var monitorRepo = new Mock<IMonitorRepository>();

            IMonitorManager monitorManager = new MonitorManager(
                It.IsAny<IChannelRepository>(),
                monitorRepo.Object,
                It.IsAny<IRecordRepository>(),
                It.IsAny<IScenarioRepository>());

            // Act
            await monitorManager.SetStatusAndResetLastTestCycleIntervalAsync(
                monitor,
                newStatus);

            // Assert
            monitorRepo.Verify(x => x.UpdateAsync(monitor), Times.Once());
            Assert.Equal(new TimeSpan?(), monitor.LastTestCycleInterval);
            Assert.Equal(newStatus, monitor.StatusEnum());
        }

        [Fact]
        public async Task SetLastTestCycleIntervalsAsync()
        {
            // Arrange
            var expectedTimeSpan = new TimeSpan(2, 40, 42);
            var expectedStatus = MonitorStatus.OK;

            var monitor = new NcMonitor
            {
                LastTestCycleInterval = It.IsAny<TimeSpan>(),
                StatusId = (int)expectedStatus,
            };

            var monitorRepo = new Mock<IMonitorRepository>();

            IMonitorManager monitorManager = new MonitorManager(
                It.IsAny<IChannelRepository>(),
                monitorRepo.Object,
                It.IsAny<IRecordRepository>(),
                It.IsAny<IScenarioRepository>());

            // Act
            await monitorManager.SetLastTestCycleIntervalsAsync(
                monitor, expectedTimeSpan);// TimeSpan je struct, neni potreba vytvaret novou referenci

            // Assert
            monitorRepo.Verify(x => x.UpdateAsyncWithoutResetTestCycleInterval(monitor), Times.Once());
            Assert.Equal(expectedTimeSpan, monitor.LastTestCycleInterval);
            Assert.Equal(expectedStatus, monitor.StatusEnum());
        }

        [Theory]
        [InlineData(5, 0)]
        [InlineData(10, 1)]
        [InlineData(19, 1)]
        [InlineData(30, 3)]
        public void GetChannelsToLastCycleTest(long monitorLastTestCycleTicks, int expectedCount)
        {
            // Arrange
            var scenarioId = 2;

            var scenarioItems = new List<NcScenarioItem>(new NcScenarioItem[]
            {
                new NcScenarioItem { ScenarioId = 1, TestCycleInterval = TimeSpan.MinValue},
                new NcScenarioItem { ScenarioId = 1, TestCycleInterval = TimeSpan.MaxValue},
                new NcScenarioItem
                {
                    ScenarioId = 2,
                    TestCycleInterval = new TimeSpan(10),
                    Channel = new NcChannel{ Id = 2 },
                },
                new NcScenarioItem
                {
                    ScenarioId = 2,
                    TestCycleInterval = new TimeSpan(20),
                    Channel = new NcChannel{ Id = 3 },
                },
                new NcScenarioItem
                {
                    ScenarioId = 2,
                    TestCycleInterval = new TimeSpan(30),
                    Channel = new NcChannel{ Id = 4 },
                },
                new NcScenarioItem { ScenarioId = 3, TestCycleInterval = TimeSpan.MinValue},
                new NcScenarioItem { ScenarioId = 3, TestCycleInterval = TimeSpan.MinValue},
                new NcScenarioItem { ScenarioId = 3, TestCycleInterval = TimeSpan.MaxValue},
            });

            var monitor = new NcMonitor
            {
                ScenarioId = scenarioId,
                LastTestCycleInterval = new TimeSpan(monitorLastTestCycleTicks),
            };

            var expected = scenarioItems
                .Where(x => x.ScenarioId == scenarioId
                    && x.TestCycleInterval <= monitor.LastTestCycleInterval)
                .Select(x=>x.Channel)
                .ToList();

            var scenarioRepo = new Mock<IScenarioRepository>();
            scenarioRepo.Setup(x => x.GetItems(scenarioId))
                .Returns((int targetId) => scenarioItems.Where(x => x.ScenarioId == targetId).AsQueryable());

            IMonitorManager monitorManager = new MonitorManager(
                It.IsAny<IChannelRepository>(),
                It.IsAny<IMonitorRepository>(),
                It.IsAny<IRecordRepository>(),
                scenarioRepo.Object);

            // Act
            var actual = monitorManager.GetChannelsToLastCycleTest(monitor);

            // Assert
            Assert.Equal(expectedCount, actual.Count);
            expected.ToExpectedObject().ShouldEqual(actual);            
        }
        
    }
}
