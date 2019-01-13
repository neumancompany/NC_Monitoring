using DevExtreme.AspNet.Mvc.Builders;
using Microsoft.AspNetCore.Http;
using NC_Monitoring.Business.Extensions;
using NC_Monitoring.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevExtreme.AspNet.Mvc
{
    public static class DevextremeBuildersExtensions
    {
        #region DataGrid"
        public static DataGridEditingBuilder<T> AllowEditingByUser<T>(this DataGridEditingBuilder<T> builder, ClaimsPrincipal user)
        {
            return builder
                .AllowAdding(user?.IsAllowAdding() ?? false)
                .AllowUpdating(user?.IsAllowUpdating() ?? false)
                .AllowDeleting(user?.IsAllowDeleting() ?? false);
        }
        #endregion
    }
}
