﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NC_Monitoring.Data.Models;

namespace NC_Monitoring.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190115192745_AddQueue")]
    partial class AddQueue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("GlobalAdmin");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelTypeId")
                        .HasColumnName("ChannelTypeID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ChannelTypeId");

                    b.ToTable("NC_Channel");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcChannelSubscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelId")
                        .HasColumnName("ChannelID");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserID");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("UserId");

                    b.ToTable("NC_ChannelSubscriber");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcChannelType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NC_ChannelType");

                    b.HasData(
                        new { Id = 2, Name = "Email" }
                    );
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcMonitor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID");

                    b.Property<TimeSpan?>("LastTestCycleInterval")
                        .HasColumnName("LastTestCycleInterval");

                    b.Property<int>("MethodTypeId")
                        .HasColumnName("MethodTypeID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("ScenarioId")
                        .HasColumnName("ScenarioID");

                    b.Property<int>("StatusId")
                        .HasColumnName("StatusID");

                    b.Property<TimeSpan>("Timeout");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<int>("VerificationTypeId")
                        .HasColumnName("VerificationTypeID");

                    b.Property<string>("VerificationValue")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("MethodTypeId");

                    b.HasIndex("ScenarioId");

                    b.HasIndex("StatusId");

                    b.HasIndex("VerificationTypeId");

                    b.ToTable("NC_Monitor");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcMonitorMethodType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NC_MonitorMethodType");

                    b.HasData(
                        new { Id = 1, Name = "GET" }
                    );
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcMonitorRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("MonitorId")
                        .HasColumnName("MonitorID");

                    b.Property<string>("Note");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("MonitorId");

                    b.ToTable("NC_MonitorRecord");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcMonitorStatusType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NC_MonitorStatusType");

                    b.HasData(
                        new { Id = 1, Name = "OK" },
                        new { Id = 2, Name = "In active" },
                        new { Id = 3, Name = "Error" }
                    );
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcMonitorVerificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Test")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NC_MonitorVerificationType");

                    b.HasData(
                        new { Id = 1, Name = "Status code" },
                        new { Id = 2, Name = "Keyword" }
                    );
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcQueue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Message");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NC_Queue");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcScenario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NC_Scenario");
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcScenarioItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelId")
                        .HasColumnName("ChannelID");

                    b.Property<int>("ScenarioId")
                        .HasColumnName("ScenarioID");

                    b.Property<TimeSpan>("TestCycleInterval");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("ScenarioId");

                    b.ToTable("NC_ScenarioItem");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NC_Monitoring.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcChannel", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.NcChannelType", "ChannelType")
                        .WithMany("NcChannel")
                        .HasForeignKey("ChannelTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcChannelSubscriber", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.NcChannel", "Channel")
                        .WithMany("NcChannelSubscriber")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NC_Monitoring.Data.Models.ApplicationUser", "User")
                        .WithMany("NcChannelSubscriber")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcMonitor", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.NcMonitorMethodType", "MethodType")
                        .WithMany("NcMonitor")
                        .HasForeignKey("MethodTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NC_Monitoring.Data.Models.NcScenario", "Scenario")
                        .WithMany("NcMonitor")
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NC_Monitoring.Data.Models.NcMonitorStatusType", "Status")
                        .WithMany("NcMonitor")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NC_Monitoring.Data.Models.NcMonitorVerificationType", "VerificationType")
                        .WithMany("NcMonitor")
                        .HasForeignKey("VerificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcMonitorRecord", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.NcMonitor", "Monitor")
                        .WithMany("NcMonitorRecord")
                        .HasForeignKey("MonitorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NC_Monitoring.Data.Models.NcScenarioItem", b =>
                {
                    b.HasOne("NC_Monitoring.Data.Models.NcChannel", "Channel")
                        .WithMany("NcScenarioItem")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NC_Monitoring.Data.Models.NcScenario", "Scenario")
                        .WithMany("NcScenarioItem")
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
