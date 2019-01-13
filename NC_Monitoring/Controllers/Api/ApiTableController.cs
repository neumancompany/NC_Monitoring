﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NC.AspNetCore.Extensions;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Exceptions;
using NC_Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Controllers.Base;
using Microsoft.AspNetCore.Authorization;

namespace NC_Monitoring.Controllers.Api
{
    public abstract class ApiTableController<TEntity, TFormViewModel, TKey> : BaseApiController
        where TEntity : class, IEntity<TKey>, new()
        where TFormViewModel : class
    {
        protected readonly IRepository<TEntity, TKey> repository;
        protected readonly IMapper mapper;

        public ApiTableController(IRepository<TEntity, TKey> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public virtual ActionResult<List<TFormViewModel>> Load()
        {
            //return DataSourceLoader.Load(mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(repository.GetAll()), loadOptions);
            //return mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(repository.GetAll()).ToList();
            return repository.GetAll().AsEnumerable().Select(x => mapper.Map<TFormViewModel>(x)).ToList();
        }

        [HttpPost("post")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public virtual async Task<IActionResult> Post(string values)
        {
            var entity = new TEntity();

            TFormViewModel viewModel;

            if (!this.TryValidateViewModelAndPopulate<TFormViewModel>(values, entity, mapper, out viewModel))
            {
                return this.GetBadRequestWithFullErrorMessage<TFormViewModel>(ModelState);
            }

            await repository.InsertAsync(entity);

            return Ok(viewModel);
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public virtual async Task<IActionResult> Put(TKey key, string values)
        {
            TEntity entity = repository.FindById(key);
            TFormViewModel viewModel;

            if (!this.TryValidateViewModelAndPopulate<TFormViewModel>(values, entity, mapper, out viewModel))
            {
                return this.GetBadRequestWithFullErrorMessage<TFormViewModel>(ModelState);
            }

            mapper.Map(viewModel, entity);

            await repository.UpdateAsync(entity);

            return Ok(viewModel);
        }

        [HttpDelete]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public virtual async Task<IActionResult> Delete(TKey key)
        {
            try
            {
                await repository.DeleteAsync(key);

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.UIMessage()); }
        }
    }
}