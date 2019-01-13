using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Controllers.Api;
using NC_Monitoring.Controllers.Base;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;

namespace NC_Monitoring.Controllers
{
    //[Route("api/scenarios")]
    public class ScenariosController : BaseController
    {
        private readonly IScenarioRepository scenarioRepository;
        private readonly IMapper mapper;

        public ScenariosController(IScenarioRepository scenarioRepository, IMapper mapper)
        {
            this.scenarioRepository = scenarioRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[Route("[controller]/{id}")]
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            ScenarioDetailViewModel model = mapper.Map<ScenarioDetailViewModel>(scenarioRepository.FindById(id));

            return View(model);
        }
    }
}