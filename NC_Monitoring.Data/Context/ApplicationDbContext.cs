using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace NC_Monitoring.Data.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}

        public virtual DbSet<NcChannel> NcChannel { get; set; }
        public virtual DbSet<NcChannelSubscriber> NcChannelSubscriber { get; set; }
        public virtual DbSet<NcChannelType> NcChannelType { get; set; }
        public virtual DbSet<NcMonitor> NcMonitor { get; set; }
        public virtual DbSet<NcMonitorMethodType> NcMonitorMethodType { get; set; }
        public virtual DbSet<NcMonitorRecord> NcMonitorRecord { get; set; }
        public virtual DbSet<NcMonitorStatusType> NcMonitorStatusType { get; set; }
        public virtual DbSet<NcMonitorVerificationType> NcMonitorVerificationType { get; set; }
        public virtual DbSet<NcScenario> NcScenario { get; set; }
        public virtual DbSet<NcScenarioItem> NcScenarioItem { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema(schema: DBGlobals.SchemaName);

            base.OnModelCreating(modelBuilder);            

            //SEED
            modelBuilder.Entity<NcChannelType>().HasData(
                //new NcChannelType
                //{
                //    Id = 1,
                //    Name = "SMS"
                //},
                new NcChannelType
                {
                    Id = 2,
                    Name = "Email",
                }
            );

            modelBuilder.Entity<NcMonitorMethodType>().HasData(
                new NcMonitorMethodType
                {
                    Id = 1,
                    Name = "GET"
                }
            );

            modelBuilder.Entity<NcMonitorStatusType>().HasData(
                new NcMonitorStatusType
                {
                    Id = 1,
                    Name = "OK"
                },
                new NcMonitorStatusType
                {
                    Id = 2,
                    Name = "In active",
                },
                new NcMonitorStatusType
                {
                    Id = 3,
                    Name = "Error",
                }
            );

            modelBuilder.Entity<NcMonitorVerificationType>().HasData(
                new NcMonitorVerificationType
                {
                    Id = 1,
                    Name = "Status code"
                },
                new NcMonitorVerificationType
                {
                    Id = 2,
                    Name = "Keyword"
                }
            );

        }
    }

}
