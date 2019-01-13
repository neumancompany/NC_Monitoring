using Microsoft.AspNetCore.Identity;
using NC_Monitoring.Business.Managers;
using NC_Monitoring.Data.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NC_Monitoring.Business.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAllowUpdating(this ClaimsPrincipal user)
        {
            return user.IsInRole(nameof(UserRole.Admin));
        }

        public static bool IsAllowAdding(this ClaimsPrincipal user)
        {
            return user.IsInRole(nameof(UserRole.Admin));
        }

        public static bool IsAllowDeleting(this ClaimsPrincipal user)
        {
            return user.IsInRole(nameof(UserRole.Admin));
        }
    }
}
