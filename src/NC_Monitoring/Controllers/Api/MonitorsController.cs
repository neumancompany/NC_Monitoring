using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Business.Interfaces;
using NC_Monitoring.Controllers.Base;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NC_Monitoring.Controllers.Api
{
    //[Route("/api/monitors")]
    public class MonitorsController : ApiTableController<NcMonitor, MonitorFormViewModel, Guid>
    {
        protected new readonly IMonitorRepository repository;
        private readonly IMonitorManager monitorManager;

        public MonitorsController(IMonitorRepository repository, IMapper mapper, IMonitorManager monitorManager)
            : base(repository, mapper)
        {
            this.repository = repository;
            this.monitorManager = monitorManager;

        }

        [HttpGet]
        [Route("methods")]
        public object GetMethodTypes()
        {
            return mapper.MapEnumerable<NcMonitorMethodType, MonitorMethodTypeDTO>(repository.GetMethodTypes());
        }

        [HttpGet]
        [Route("verifications")]
        public IActionResult GetVerificationTypes()
        {
            return Ok(mapper.MapEnumerable<MonitorVerificationTypeDTO>(repository.GetVerificationTypes()).ToList());
        }

        public class MonitorStatusOutput
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [HttpGet]
        [Route("statuses")]
        public List<MonitorStatusOutput> LoadStatuses()
        {
            return repository.GetStatusTypes().Select(x=> new MonitorStatusOutput
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }

        [HttpGet("MethodTypesSelectList")]
        public LoadResult MethodTypesSelectList(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(repository.GetMethodTypes().ToSelectList(x => x.Id, x => x.Name), loadOptions);
        }

        [HttpGet("VerificationTypesSelectList")]
        public LoadResult VerificationTypesSelectList(DataSourceLoadOptions loadOptions)
        {
            //return mapper.Map<List<ISelectItem<int>>>(repository.GetVerificationTypes());
            return DataSourceLoader.Load(repository.GetVerificationTypes().ToSelectList(x => x.Id, x => x.Name), loadOptions);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var detail = repository.FindById(id);

            if (detail == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<NcMonitor, MonitorDetailDTO>(detail));
        }

        #region "Records"

        [Route("/api/records/load")]
        public IActionResult RecordsLoad(DataSourceLoadOptions loadOptions)
        {
            return Ok(DataSourceLoader.Load(mapper.MapQueryable<MonitorRecordListDTO>(
                monitorManager
                    .GetAllRecords()
                    .OrderByDescending(x => x.StartDate)),
                loadOptions).data);
        }

        [Route("/api/records/ForMonitorLoad", Name = "RecordsForMonitorLoad")]
        public IActionResult RecordsForMonitorLoad(Guid monitorId, DataSourceLoadOptionsBase options)
        {
            return Ok(DataSourceLoader.Load(mapper.MapQueryable<NcMonitorRecord, MonitorRecordListDTO>(monitorManager
                .GetRecordsForMonitor(monitorId)
                .OrderByDescending(x => x.StartDate)), options).data);
        }


        [HttpDelete("/api/records/clear/old/{monitorId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult ClearOldRecordsForMonitor(Guid monitorId)
        {
            monitorManager.DeleteOldRecordsForMonitorAsync(monitorId).Wait();
            return Ok();
        }

        [HttpDelete("/api/records/clear/{monitorId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult ClearRecordsForMonitor(Guid monitorId)
        {
            monitorManager.DeleteRecordsForMonitorAsync(monitorId).Wait();
            return Ok();
        }

        [HttpDelete("/api/records/clear/old")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult ClearAllOldRecords()
        {
            monitorManager.DeleteOldRecordsAsync().Wait();
            return Ok();
        }

        [HttpDelete("/api/records/clear")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult ClearAllRecords()
        {
            monitorManager.DeleteRecordsAsync().Wait();
            return Ok();
        }

        #endregion
    }
}
