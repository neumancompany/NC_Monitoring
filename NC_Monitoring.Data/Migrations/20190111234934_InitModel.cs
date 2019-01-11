using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NC_Monitoring.Data.Migrations
{
    public partial class InitModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    GlobalAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NC_ChannelType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_ChannelType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NC_MonitorMethodType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_MonitorMethodType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NC_MonitorStatusType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_MonitorStatusType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NC_MonitorVerificationType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Test = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_MonitorVerificationType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NC_Scenario",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_Scenario", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NC_Channel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    ChannelTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_Channel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NC_Channel_NC_ChannelType_ChannelTypeID",
                        column: x => x.ChannelTypeID,
                        principalTable: "NC_ChannelType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NC_Monitor",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    StatusID = table.Column<int>(nullable: false),
                    MethodTypeID = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 250, nullable: false),
                    VerificationTypeID = table.Column<int>(nullable: false),
                    VerificationValue = table.Column<string>(maxLength: 250, nullable: false),
                    Timeout = table.Column<TimeSpan>(nullable: false),
                    ScenarioID = table.Column<int>(nullable: false),
                    LastTestCycleInterval = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_Monitor", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NC_Monitor_NC_MonitorMethodType_MethodTypeID",
                        column: x => x.MethodTypeID,
                        principalTable: "NC_MonitorMethodType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NC_Monitor_NC_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "NC_Scenario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NC_Monitor_NC_MonitorStatusType_StatusID",
                        column: x => x.StatusID,
                        principalTable: "NC_MonitorStatusType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NC_Monitor_NC_MonitorVerificationType_VerificationTypeID",
                        column: x => x.VerificationTypeID,
                        principalTable: "NC_MonitorVerificationType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NC_ChannelSubscriber",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<int>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_ChannelSubscriber", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NC_ChannelSubscriber_NC_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "NC_Channel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NC_ChannelSubscriber_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NC_ScenarioItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScenarioID = table.Column<int>(nullable: false),
                    ChannelID = table.Column<int>(nullable: false),
                    TestCycleInterval = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_ScenarioItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NC_ScenarioItem_NC_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "NC_Channel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NC_ScenarioItem_NC_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "NC_Scenario",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NC_MonitorRecord",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MonitorID = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NC_MonitorRecord", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NC_MonitorRecord_NC_Monitor_MonitorID",
                        column: x => x.MonitorID,
                        principalTable: "NC_Monitor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NC_ChannelType",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Email" });

            migrationBuilder.InsertData(
                table: "NC_MonitorMethodType",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "GET" });

            migrationBuilder.InsertData(
                table: "NC_MonitorStatusType",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "OK" },
                    { 2, "In active" },
                    { 3, "Error" }
                });

            migrationBuilder.InsertData(
                table: "NC_MonitorVerificationType",
                columns: new[] { "ID", "Name", "Test" },
                values: new object[,]
                {
                    { 1, "Status code", null },
                    { 2, "Keyword", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NC_Channel_ChannelTypeID",
                table: "NC_Channel",
                column: "ChannelTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_ChannelSubscriber_ChannelID",
                table: "NC_ChannelSubscriber",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_ChannelSubscriber_UserID",
                table: "NC_ChannelSubscriber",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_Monitor_MethodTypeID",
                table: "NC_Monitor",
                column: "MethodTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_Monitor_ScenarioID",
                table: "NC_Monitor",
                column: "ScenarioID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_Monitor_StatusID",
                table: "NC_Monitor",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_Monitor_VerificationTypeID",
                table: "NC_Monitor",
                column: "VerificationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_MonitorRecord_MonitorID",
                table: "NC_MonitorRecord",
                column: "MonitorID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_ScenarioItem_ChannelID",
                table: "NC_ScenarioItem",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_NC_ScenarioItem_ScenarioID",
                table: "NC_ScenarioItem",
                column: "ScenarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "NC_ChannelSubscriber");

            migrationBuilder.DropTable(
                name: "NC_MonitorRecord");

            migrationBuilder.DropTable(
                name: "NC_ScenarioItem");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "NC_Monitor");

            migrationBuilder.DropTable(
                name: "NC_Channel");

            migrationBuilder.DropTable(
                name: "NC_MonitorMethodType");

            migrationBuilder.DropTable(
                name: "NC_Scenario");

            migrationBuilder.DropTable(
                name: "NC_MonitorStatusType");

            migrationBuilder.DropTable(
                name: "NC_MonitorVerificationType");

            migrationBuilder.DropTable(
                name: "NC_ChannelType");
        }
    }
}
