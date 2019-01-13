using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Exceptions
{
    public class UIException : Exception
    {
        public UIException(string message) : base(message) { }

        public UIException(string message, Exception innerException) : base(message, innerException) { }

    }
}
