using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NC_Monitoring.ConsoleApp.WebJobs
{
    public class JobActivator : IJobActivator
    {
        private readonly IServiceProvider services;

        public JobActivator(IServiceProvider services)
        {
            this.services = services;
        }

        public T CreateInstance<T>()
        {
            return services.GetService<T>();
        }
    }
}