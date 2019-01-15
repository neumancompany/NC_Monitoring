using Microsoft.Extensions.Logging;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NC_Monitoring.ConsoleApp.Classes
{
    /// <summary>
    /// Prace s frontou v databazi.
    /// </summary>
    public class QueueProvider
    {
        private const string PARAMS_SEPARATOR = ";";

        private readonly ApplicationDbContext context;
        private readonly ILogger<QueueProvider> logger;

        public QueueProvider(ApplicationDbContext context, ILogger<QueueProvider> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task PushAsync(QueueType type, object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"Parameter {nameof(obj)} of method {nameof(PushAsync)} cannot be null.");
            }

            context.NcQueue.Add(new NcQueue { Type = type, Message = JsonConvert.SerializeObject(obj) });
            await context.SaveChangesAsync();
        }

        public async Task<T> PopAsync<T>(QueueType type)
        {
            NcQueue item = context.NcQueue.Where(x => x.Type == type).OrderBy(x => x.Id).FirstOrDefault();

            if (item == null)
            {
                return default(T);
            }

            string message = item.Message;

            context.NcQueue.Remove(item);
            await context.SaveChangesAsync();

            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}