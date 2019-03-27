using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.Extensions;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.Controllers.Base;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_Monitoring.Controllers.Api
{
    public class UsersController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly ApplicationRoleManager roleManager;
        private readonly IMapper mapper;
        private readonly IEmailNotificator emailNotificator;

        public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager, IMapper mapper, IEmailNotificator emailNotificator)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.emailNotificator = emailNotificator;
        }

        [HttpGet]
        public async Task<List<UserViewModel>> Load()
        {
            Dictionary<Guid, ApplicationUser> appUsers = userManager.Users.ToDictionary(k => k.Id, v => v);
            List<UserViewModel> users = mapper.MapEnumerable<UserViewModel>(userManager.Users).ToList();

            ApplicationUser currentUser = await userManager.GetUserAsync(User);
            Guid? currentUserId = currentUser?.Id;
            bool isGlobalAdminCurrentUser = currentUser?.GlobalAdmin ?? false;

            foreach (UserViewModel user in users)
            {
                ApplicationUser appUser = appUsers[user.Id];
                bool isCurrentUser = currentUserId == appUser.Id;

                user.RoleName = (await userManager.GetRolesAsync(appUser)).FirstOrDefault();

                bool isAdmin = user.RoleName == nameof(UserRole.Admin);

                user.AllowResetPassword = !isCurrentUser && User.IsAllowUpdating()
                    && (!appUser.GlobalAdmin || isGlobalAdminCurrentUser);

                if (isCurrentUser || user.GlobalAdmin)
                {
                    user.AllowEditing = user.AllowDeleting = false;
                }
                else
                {
                    user.AllowEditing = isGlobalAdminCurrentUser && User.IsAllowUpdating();
                    user.AllowDeleting = isGlobalAdminCurrentUser && User.IsAllowDeleting();
                }
            }

            return users;
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> ResetPassword(Guid key)
        {
            ApplicationUser foundUser = await userManager.FindByIdAsync(key);

            if (foundUser == null)
            {
                return BadRequest("User was not found.");
            }

            ApplicationUser currentUser = await userManager.GetUserAsync(User);

            if (foundUser.GlobalAdmin && !currentUser.GlobalAdmin)
            {
                return Forbid();
            }

            string newPassword = Guid.NewGuid().ToString().Replace("-", "");

            IdentityResult result = await userManager.ResetPasswordAsync(foundUser, await userManager.GeneratePasswordResetTokenAsync(foundUser), newPassword);

            if (!result.Succeeded)
            {
                return BadRequest("Reset password failed.");
            }

            await emailNotificator.SendEmailAsync(foundUser.Email, "Reset password",
                $"Your password was reset.\n\nUserName: {foundUser.UserName}\nNew password: {newPassword}");

            return Ok($"New password was send to email. ({foundUser.Email})");
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Put(Guid key, string values)
        {
            UserViewModel userModel = new UserViewModel();
            JsonConvert.PopulateObject(values, userModel);

            ApplicationUser foundUser = await userManager.FindByIdAsync(key);

            if (foundUser == null)
            {
                return BadRequest("User not found.");
            }

            ApplicationUser currentUser = await userManager.GetUserAsync(User);

            if (foundUser.GlobalAdmin)
            {
                return Forbid();
            }

            await userManager.RemoveFromRolesAsync(foundUser, await userManager.GetRolesAsync(foundUser));
            await userManager.AddToRoleAsync(foundUser, userModel.RoleName);

            return Ok(key);
        }

        [HttpDelete]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete(Guid key)
        {
            ApplicationUser foundUser = await userManager.FindByIdAsync(key);

            if (foundUser == null)
            {
                return BadRequest("User not found.");
            }

            ApplicationUser currentUser = await userManager.GetUserAsync(User);

            if (foundUser.GlobalAdmin)
            {
                return Forbid();
            }

            await userManager.DeleteAsync(foundUser);

            return Ok(key);
        }

        [HttpGet]
        public LoadResult RolesSelectList(DataSourceLoadOptions options)
        {
            return DataSourceLoader.Load(roleManager.Roles.ToSelectList(x => x.Name, x => x.Name), options);
        }
    }
}