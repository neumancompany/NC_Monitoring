using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace NC_Monitoring.Data.Generated
{
    public partial class NC_MONITORINGContext : DbContext
    {
        private readonly IConfiguration configuration;

        public NC_MONITORINGContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
