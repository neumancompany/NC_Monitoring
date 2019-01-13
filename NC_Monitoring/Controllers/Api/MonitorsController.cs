using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
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

        public LoadResult MethodTypesSelectList(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(repository.GetMethodTypes().ToSelectList(x => x.Id, x => x.Name), loadOptions);
        }

        public LoadResult VerificationTypesSelectList(DataSourceLoadOptions loadOptions)
        {
            //return mapper.Map<List<ISelectItem<int>>>(repository.GetVerificationTypes());
            return DataSourceLoader.Load(repository.GetVerificationTypes().ToSelectList(x => x.Id, x => x.Name), loadOptions);
        }

        #region "Records"

        [Route("/api/records/load")]
        public LoadResult RecordsLoad(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(mapper.MapQueryable<MonitorRecordListDTO>(monitorManager.GetAllRecords().OrderByDescending(x => x.StartDate)), loadOptions);
        }
        [Route("/api/records/ForMonitorLoad", Name = "RecordsForMonitorLoad")]
        public IEnumerable<MonitorRecordListDTO> RecordsForMonitorLoad(Guid monitorId)
        {
            return mapper.MapEnumerable<NcMonitorRecord, MonitorRecordListDTO>(monitorManager
                .GetRecordsForMonitor(monitorId)
                .OrderByDescending(x=>x.StartDate));
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
