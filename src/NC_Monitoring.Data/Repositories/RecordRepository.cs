using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Repositories
{
    public class RecordRepository : BaseRepository<NcMonitorRecord, int>, IRecordRepository
    {
        public RecordRepository(ApplicationDbContext context)
            : base(context)
        { }

        public async Task DeleteAsync(Expression<Func<NcMonitorRecord, bool>> expression)
        {
            context.NcMonitorRecord.RemoveRange(context.NcMonitorRecord.Where(expression));

            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync()
        {
            context.NcMonitorRecord.RemoveRange(context.NcMonitorRecord);

            await context.SaveChangesAsync();
        }
    }
}
