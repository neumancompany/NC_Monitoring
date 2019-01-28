using Microsoft.Extensions.Logging;
using Moq;
using NC.Extensions;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.ConsoleApp.Classes;
using NC_Monitoring.ConsoleApp.Interfaces;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NC_Monitoring.Test.ConsoleApp
{
    public class MonitorRecorderTest
    {
        [Theory]
        // zmena z erroru na OK
        [InlineData(MonitorStatus.Error, MonitorStatus.OK, true, true)] 
        [InlineData(MonitorStatus.Error, MonitorStatus.OK, true, false)]
        [InlineData(MonitorStatus.Error, MonitorStatus.OK, false, true)]
        [InlineData(MonitorStatus.Error, MonitorStatus.OK, false, false)]

        // zmena z OK na error
        [InlineData(MonitorStatus.OK, MonitorStatus.Error, true, true)]
        [InlineData(MonitorStatus.OK, MonitorStatus.Error, true, false)]
        [InlineData(MonitorStatus.OK, MonitorStatus.Error, false, true)]
        [InlineData(MonitorStatus.OK, MonitorStatus.Error, false, false)]

        // monitor je stale v chybovem stavu
        [InlineData(MonitorStatus.Error, MonitorStatus.Error, true, true)]
        [InlineData(MonitorStatus.Error, MonitorStatus.Error, true, false)]
        [InlineData(MonitorStatus.Error, MonitorStatus.Error, false, true)]
        [InlineData(MonitorStatus.Error, MonitorStatus.Error, false, false)]

        // monitor je stale v OK stavu
        [InlineData(MonitorStatus.OK, MonitorStatus.OK, true, true)]
        [InlineData(MonitorStatus.OK, MonitorStatus.OK, true, false)]
        [InlineData(MonitorStatus.OK, MonitorStatus.OK, false, true)]
        [InlineData(MonitorStatus.OK, MonitorStatus.OK, false, false)]
        public async Task RecordAsync(MonitorStatus oldStatus, MonitorStatus monitorResult, bool existLastRecord, bool lastRecordIsOpen)
        {
            // Arrange
            var monitorResultSuccess = monitorResult != MonitorStatus.Error;
            var lastRecordEndDate = existLastRecord && !lastRecordIsOpen 
                ? new DateTime?(DateTime.MinValue) 
                : new DateTime?();
            
            var monitor = new NcMonitor
            {
                Id = Guid.NewGuid(),
                StatusId = (int)oldStatus,
            };
            var mRecord = new NcMonitorRecord
            {
                EndDate = lastRecordEndDate
            };
            var mResult = new MonitorResult
            {
                IsSuccess = monitorResultSuccess
            };

            var monitorManager = new Mock<IMonitorManager>();
            monitorManager.Setup(x => x.LastOpenedMonitorRecord(monitor.Id))
                .Returns(() =>
                {
                    if (!existLastRecord || !lastRecordIsOpen)
                    {
                        return null;
                    }

                    return mRecord;
                }).Verifiable();

            monitorManager.Setup(x => x.UpdateEndDateAsync(mRecord, It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            monitorManager.Setup(x => 
                x.SetStatusAndResetLastTestCycleIntervalAsync(monitor, monitorResult))
                    .Returns(Task.CompletedTask);

            var notificator = new Mock<INotificator>();
            notificator.Setup(x => x.AddNotifyUpAsync(monitor)).Returns(Task.CompletedTask);
            notificator.Setup(x => x.AddNotifyDownAsync(monitor)).Returns(Task.CompletedTask);

            var monitorRecorder = new MonitorRecorder(
                It.IsAny<ILogger<MonitorRecorder>>(),
                monitorManager.Object,
                notificator.Object);

            // Act
            await monitorRecorder.RecordAsync(monitor, mResult);

            // Assert
            monitorManager.Verify();

            if (monitorResultSuccess)
            {
                if ((existLastRecord && lastRecordIsOpen) || oldStatus != MonitorStatus.OK)
                {//ukonceni zaznamu
                    if (existLastRecord && lastRecordIsOpen)
                    {
                        monitorManager.Verify(x => x.UpdateEndDateAsync(mRecord, It.IsAny<DateTime>()));
                    }

                    notificator.Verify(x => x.AddNotifyUpAsync(monitor));
                    monitorManager.Verify(x => x.SetStatusAndResetLastTestCycleIntervalAsync(monitor, MonitorStatus.OK));
                }
            }
            else
            {
                if (!existLastRecord || !lastRecordIsOpen || oldStatus != MonitorStatus.Error)
                {//neexistuje zadny zaznam nebo jsou vsechny uzavreny nebo aktualni status monitoru neni v chybovem stavu
                    monitorManager.Verify(x => x.SetStatusAndResetLastTestCycleIntervalAsync(monitor, MonitorStatus.Error));
                }
                notificator.Verify(x => x.AddNotifyDownAsync(monitor));
            }
        }
    }
}
