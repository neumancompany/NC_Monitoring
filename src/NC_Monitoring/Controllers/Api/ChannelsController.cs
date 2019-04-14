using AutoMapper;
using Castle.Core.Logging;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NC_Monitoring.Controllers.Api
{
    public class ChannelsController : ApiTableController<NcChannel, ChannelViewModel, int>
    {
        //protected new readonly IChannelRepository repository;
        private readonly IChannelManager channelManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ChannelsController> logger;

        public ChannelsController(ILogger<ChannelsController> logger, IChannelRepository repository, IMapper mapper, IChannelManager channelManager, UserManager<ApplicationUser> userManager) : base(repository, mapper)
        {
            this.channelManager = channelManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet("selectList")]
        public LoadResult SelectList(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(repository.GetAll().ToSelectList(x => x.Id, x => x.Name), loadOptions);
        }

        [HttpGet("TypesSelectList")]
        public LoadResult TypesSelectList(DataSourceLoadOptions loadOptions)
        {
            //return repository.GetChannelTypes().ToSelectList(x => x.Id, x => x.Name);
            return DataSourceLoader.Load(channelManager.GetChannelTypes().ToSelectList(x => x.Id, x => x.Name), loadOptions);
        }

        [HttpGet("UserSelectList")]
        public LoadResult UserSelectList(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(userManager.Users.ToSelectList(x => x.Id, x => x.Email), loadOptions);
        }

        //public LoadResult UserSelectList(DataSourceLoadOptions loadOptions, int channelId)
        //{
        //    return DataSourceLoader.Load(channelManager.GetUsersNotAssignedToChannelYet(channelId).ToSelectList(x=>x.Id, x=>x.UserName), loadOptions);
        //}

        #region "Subscribers"

        [HttpGet("SubscriberLoad")]
        public IEnumerable<ChannelSubscriberViewModel> SubscriberLoad(int channelId)
        {
            return mapper.MapEnumerable<NcChannelSubscriber, ChannelSubscriberViewModel>(channelManager.GetSubscribersByChannel(channelId));
        }

        [HttpPost("SubscriberPost")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public virtual IActionResult SubscriberPost(string values)
        {
            var entity = new NcChannelSubscriber();

            if (!this.TryValidateViewModel(values, entity))
            {
                return this.GetBadRequestWithFullErrorMessage<NcChannelSubscriber>(ModelState);
            }

            return channelManager.SubscriberInsertAsync(entity).WaitForActionResult();
        }

        [HttpPut("SubscriberPut")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public virtual IActionResult SubscriberPut(int key, string values)
        {
            NcChannelSubscriber entity = channelManager.FindSubscriberById(key);

            if (!this.TryValidateViewModel(values, entity))
            {
                return this.GetBadRequestWithFullErrorMessage<NcChannelSubscriber>(ModelState);
            }

            //await channelManager.SubscriberUpdateAsync(entity).TryCatchAsync(ex=>logger.LogError(ex, "CHYBA"));
            return channelManager.SubscriberUpdateAsync(entity).WaitForActionResult();
        }

        [HttpDelete("SubscriberDelete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public virtual IActionResult SubscriberDelete(int key)
        {
            return channelManager.SubscriberDeleteAsync(key).WaitForActionResult();
        }

        #endregion
    }
}
