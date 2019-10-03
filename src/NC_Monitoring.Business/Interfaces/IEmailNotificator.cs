using System;
using System.Threading.Tasks;

namespace NC_Monitoring.Business.Interfaces
{
    public interface IEmailNotificator
    {
        Task SendEmailErrorAsync(string message, params object[] data);

        Task SendEmailExceptionAsync(Exception ex, string message, params object[] data);

        Task SendEmailAsync(string emailTo, string subject, string message);
    }
}