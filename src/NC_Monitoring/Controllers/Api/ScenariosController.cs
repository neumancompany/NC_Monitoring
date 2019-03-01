using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NC_Monitoring.Controllers.Api
{
    public class ScenariosController : ApiTableController<NcScenario, ScenarioViewModel, int>
    {
        private readonly IScenarioRepository scenarioRepository;

        public ScenariosController(IScenarioRepository scenarioRepository, IMapper mapper)
            : base(scenarioRepository, mapper)
        {
            this.scenarioRepository = scenarioRepository;
        }

        public LoadResult SelectList(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(Load().Value.ToSelectList(x => x.Id, x => x.Name), loadOptions);
        }

        public IEnumerable<ScenarioItemViewModel> ItemsLoad(int id)
        {
            return mapper.MapEnumerable<NcScenarioItem, ScenarioItemViewModel>(
                scenarioRepository.GetItems(id).OrderBy(x => x.TestCycleInterval));
        }

        [HttpPost]
        public async Task<IActionResult> ItemsPost(string values)
        {
            var entity = new NcScenarioItem();

            ScenarioItemViewModel viewModel;

            if (!this.TryValidateViewModelAndPopulate(values, entity, mapper, out viewModel))
            {
                return this.GetBadRequestWithFullErrorMessage<ScenarioItemViewModel>(ModelState);
            }

            await scenarioRepository.InsertItemAsync(entity);

            return Ok(viewModel);
        }

        [HttpPut]
        public async Task<IActionResult> ItemsPut(int key, string values)
        {
            var entity = scenarioRepository.FindItemById(key);

            ScenarioItemViewModel viewModel;

            if (!this.TryValidateViewModelAndPopulate(values, entity, mapper, out viewModel))
            {
                return this.GetBadRequestWithFullErrorMessage<ScenarioItemViewModel>(ModelState);
            }

            await scenarioRepository.UpdateItemAsync(entity);

            return Ok(viewModel);
        }

        [HttpDelete]
        [Authorize]
        public async Task ItemsDelete(int key)
        {
            await scenarioRepository.DeleteItemAsync(key);
        }
    }
}
