using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Enums
{
    public enum SqlExceptionCodeEnum
    {
        DeleteConstraint            = 547,
        UniqueConstraint            = 2627,
        UniqueIndex                 = 2601,
    }
}
