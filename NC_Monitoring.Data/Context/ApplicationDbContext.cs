using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NC_Monitoring.Data.Enums;
using System;

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

        public virtual DbSet<NcQueue> NcQueue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NcQueue>()
                .Property(e => e.Type)
                .HasConversion(new EnumToStringConverter<QueueType>());

            //SEED
            modelBuilder.Entity<NcChannelType>().HasData(                
                new NcChannelType
                {
                    Id = (int)ChannelType.Email,
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
                    Id = (int)MonitorStatus.OK,
                    Name = "OK"
                },
                new NcMonitorStatusType
                {
                    Id = (int)MonitorStatus.InActive,
                    Name = "In active",
                },
                new NcMonitorStatusType
                {
                    Id = (int)MonitorStatus.Error,
                    Name = "Error",
                }
            );

            modelBuilder.Entity<NcMonitorVerificationType>().HasData(
                new NcMonitorVerificationType
                {
                    Id = (int)MonitorVerification.StatusCode,
                    Name = "Status code"
                },
                new NcMonitorVerificationType
                {
                    Id = (int)MonitorVerification.Keyword,
                    Name = "Keyword"
                }
            );

            // plnění testovacích dat

            modelBuilder.Entity<NcChannel>().HasData(
                new NcChannel { Id = 1, Name = "Podpora" },
                new NcChannel { Id = 2, Name = "Správci" }
            );

            //modelBuilder.Entity<NcChannelSubscriber>().HasData(
            //    // nelze ziskat ID uzivatele, jelikoz je to GUID a vytvori se az pri inicializaci DB
            //    //new NcChannelSubscriber { Id = 1, , UserId = 1, ChannelId = 1 },
            //    //new NcChannelSubscriber { Id = 2, UserId = 1, ChannelId = 1 } 
            //);

            modelBuilder.Entity<NcScenario>().HasData(
                new NcScenario { Id = 1, Name = "Scénář 1" },
                new NcScenario { Id = 2, Name = "Scénář 2" }
            );

            modelBuilder.Entity<NcScenarioItem>().HasData(
                new NcScenarioItem
                {
                    Id = 1,
                    ChannelId = 1,
                    TestCycleInterval = TimeSpan.Zero
                },
                new NcScenarioItem
                {
                    Id = 2,
                    ChannelId = 2,
                    TestCycleInterval = new TimeSpan(0, 10, 0), // upozorneni po 10 min vypadku
                }
            );

            modelBuilder.Entity<NcMonitor>().HasData(
                new NcMonitor
                {
                    Id = Guid.NewGuid(),
                    Name = "Seznam - kontrola status code",
                    MethodTypeId = 1,
                    VerificationTypeId = (int)MonitorVerification.StatusCode,
                    VerificationValue = "200",
                    Timeout = new TimeSpan(0, 1, 0), // 1 min.
                    Url = "https://seznam.cz",
                    ScenarioId = 1,
                },
                new NcMonitor
                {
                    Id = Guid.NewGuid(),
                    Name = "Google - kontrola keyword",
                    MethodTypeId = 1,
                    VerificationTypeId = (int)MonitorVerification.Keyword,
                    VerificationValue = "Google",
                    Timeout = new TimeSpan(0, 1, 0), // 1 min.
                    Url = "https://google.com",
                    ScenarioId = 1,
                },
                new NcMonitor
                {
                    Id = Guid.NewGuid(),
                    Name = "Facebook - kontrola keyword",
                    MethodTypeId = 1,
                    VerificationTypeId = (int)MonitorVerification.Keyword,
                    VerificationValue = "facebook",
                    Timeout = new TimeSpan(0, 1, 0), // 1 min.
                    Url = "https://facebook.com",
                    ScenarioId = 2,
                }
            );


            base.OnModelCreating(modelBuilder);
        }
    }

}
