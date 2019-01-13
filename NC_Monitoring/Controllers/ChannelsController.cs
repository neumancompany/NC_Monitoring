using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Controllers.Api;
using NC_Monitoring.Controllers.Base;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_Monitoring.Controllers
{
    public class ChannelsController : BaseController
    {
        private readonly IChannelManager channelManager;
        private readonly IMapper mapper;

        public ChannelsController(IChannelManager channelManager, IMapper mapper)
        {
            this.channelManager = channelManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var channel = channelManager.FindById(id);
            ChannelViewModel model = mapper.Map<ChannelViewModel>(channel);            

            return View(model);
        }
    }
}