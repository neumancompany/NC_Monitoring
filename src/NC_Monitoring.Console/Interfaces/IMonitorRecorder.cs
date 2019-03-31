using System.Threading.Tasks;
using NC_Monitoring.ConsoleApp.Classes;
using NC_Monitoring.Data.Models;

namespace NC_Monitoring.ConsoleApp.Interfaces
{
    public interface IMonitorRecorder
    {
        Task RecordAsync(NcMonitor monitor, MonitorResult result);
    }
}