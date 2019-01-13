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
    }
}
