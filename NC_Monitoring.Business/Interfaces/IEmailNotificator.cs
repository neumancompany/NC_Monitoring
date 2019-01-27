using System.Threading.Tasks;

namespace NC_Monitoring.Business.Interfaces
{
    public interface IEmailNotificator
    {
        Task SendEmailAsync(string emailTo, string subject, string message);
    }
}