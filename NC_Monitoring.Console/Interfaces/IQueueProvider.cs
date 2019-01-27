using System.Threading.Tasks;
using NC_Monitoring.Data.Enums;

namespace NC_Monitoring.ConsoleApp.Interfaces
{
    public interface IQueueProvider
    {
        Task<T> PopAsync<T>(QueueType type);
        Task PushAsync(QueueType type, object obj);
    }
}