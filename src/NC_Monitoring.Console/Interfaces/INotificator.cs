using System.Threading.Tasks;
using NC_Monitoring.Data.Models;

namespace NC_Monitoring.ConsoleApp.Interfaces
{
    public interface INotificator
    {
        Task AddNotifyDownAsync(NcMonitor monitor);
        Task AddNotifyUpAsync(NcMonitor monitor);
        Task SendAllNotifications();
    }
}