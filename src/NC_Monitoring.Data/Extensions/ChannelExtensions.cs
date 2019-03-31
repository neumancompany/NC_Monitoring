using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Extensions
{
    public static class ChannelExtensions
    {
        public static ChannelType ChannelTypeEnum(this NcChannel channel)
        {
            return (ChannelType)channel.ChannelTypeId;
        }
    }
}
