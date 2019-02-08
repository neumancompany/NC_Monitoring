using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using System;
using System.Linq;

namespace NC_Monitoring
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            var userManager = services.GetService<UserManager<ApplicationUser>>();
            var roleManager = services.GetService<RoleManager<ApplicationRole>>();
            var configuration = services.GetService<IConfiguration>();

            SeedRoles(roleManager);
            SeedUsers(userManager);

            SeedGlobalAdmins(userManager, configuration);
        }

        private static void SeedGlobalAdmins(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            var globalAdmins = configuration.GetSection("GlobalAdmins")
                .GetChildren()
                .ToList()
                .Select(x => (
                    x.GetValue<string>("UserName"),
                    x.GetValue<string>("Password")))
                //.ToList<(string UserName, string Password)>()
                .ToDictionary(k => k.Item1, k => k.Item2, StringComparer.InvariantCultureIgnoreCase);

            foreach (var globalAdmin in globalAdmins)
            {
                ApplicationUser user = userManager.AddUser(globalAdmin.Key, globalAdmin.Value, nameof(UserRole.Admin));

                if (!user.GlobalAdmin)
                {
                    user.GlobalAdmin = true;
                    userManager.UpdateAsync(user).Wait();
                }
            }


            foreach (ApplicationUser user in userManager.Users.Where(x => !globalAdmins.ContainsKey(x.UserName)))
            {
                if (user.GlobalAdmin)
                {
                    user.GlobalAdmin = false;
                    userManager.UpdateAsync(user).Wait();
                }
            }
        }

        private static void SeedRoles(this RoleManager<ApplicationRole> roleManager)
        {
            roleManager.AddRole(nameof(UserRole.Admin));
            roleManager.AddRole(nameof(UserRole.User));
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            //userManager.AddUser("admin@admin.cz", "admin12", nameof(UserRole.Admin));
        }

        private static bool AddRole(this RoleManager<ApplicationRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                var result = roleManager.CreateAsync(new ApplicationRole()
                {
                    Name = roleName,
                }).Result;

                return result.Succeeded;
            }

            return false;
        }

        private static ApplicationUser AddUser(this UserManager<ApplicationUser> userManager, string email, string password, string roleName)
        {
            return AddUser(userManager, email, email, password, roleName);
        }

        private static ApplicationUser AddUser(this UserManager<ApplicationUser> userManager, string username, string email, string password, string roleName)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cant be null.");
            }

            var user = userManager.FindByNameAsync(username).Result;

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = username,
                    Email = email,
                };

                userManager.CreateAsync(user).Wait();
                userManager.AddPasswordAsync(user, password).Wait();
            }

            userManager.AddToRoleAsync(user, roleName).Wait();

            return user;
        }
    }
}
