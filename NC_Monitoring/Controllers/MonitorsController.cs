using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Controllers.Base;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NC_Monitoring.Controllers
{
    public class MonitorsController : BaseController
    {
        private readonly IMapper mapper;
        private readonly IMonitorManager monitorManager;

        public MonitorsController(IMonitorManager monitorManager, IMapper mapper)
        {
            this.monitorManager = monitorManager;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Details(Guid id)
        {
            var model = mapper.Map<MonitorDetailDTO>(monitorManager.FindMonitor(id));
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}