using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NC_Monitoring.Business.Classes;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.Data.Repositories;
using NC_Monitoring.Mapper;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace NC_Monitoring
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Shared");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                //.AddJsonFile(Path.Combine(sharedFolder, "sharedsettings.json"), optional: true)
                //.AddJsonFile(Path.Combine(sharedFolder, $"sharedsettings.{env.EnvironmentName}.json"), optional: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(o =>
               {//vyzadani globalni autorizace na vsech strankach, ktere nemaji atribut [AllowAnonymous]
                   var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();

                   o.Filters.Add(new AuthorizeFilter(policy));
               })
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSession();

            services.AddAutoMapper(cfg => MapperConfigure.Configure(cfg));

            services
                .AddDbContext<ApplicationDbContext>(options => options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("NC_Monitoring.Data")));
            //Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.Stores.MaxLengthForKeys = 128)
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                // .AddDefaultUI()//vytvoreni stranek pro prihlaseni /Ideneity/Account/Login atd.
                .AddDefaultTokenProviders()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddRoles<ApplicationRole>();

            services
                .Configure<IdentityOptions>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 2;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;
                });

            services
                .AddTransient<IMonitorRepository, MonitorRepository>()
                .AddTransient<IMonitorManager, MonitorManager>()

                .AddTransient<IRecordRepository, RecordRepository>()

                .AddTransient<IChannelManager, ChannelManager>()
                .AddTransient<IChannelRepository, ChannelRepository>()

                .AddTransient<IScenarioRepository, ScenarioRepository>();
            
            services
                .AddTransient<EmailNotificator>()
                .AddScoped<SmtpClient>(conf =>
                {
                    return new SmtpClient()
                    {
                        Host = Configuration.GetValue<string>("Email:Smtp:Host"),
                        Port = Configuration.GetValue<int>("Email:Smtp:Port"),
                        Credentials = new NetworkCredential(
                            Configuration.GetValue<string>("Email:Smtp:Username"),
                            Configuration.GetValue<string>("Email:Smtp:Password")
                        )
                    };
                });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            // Uncomment to use pre-17.2 routing for .Mvc() and .WebApi() data sources
            // DevExtreme.AspNet.Mvc.Compatibility.DataSource.UseLegacyRouting = true;
            // Uncomment to use pre-17.2 behavior for the "required" validation check
            // DevExtreme.AspNet.Mvc.Compatibility.Validation.IgnoreRequiredForBoolean = false;

            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();

                loggerFactory.AddDebug();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "api",
                    template: "api/{controller}/{action=load}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
        }
    }
}