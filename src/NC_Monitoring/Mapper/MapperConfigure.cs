using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NC.AspNetCore.Interfaces;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Extensions;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NC_Monitoring.Mapper
{
    public static class MapperConfigure
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<MonitorFormViewModel, NcMonitor>()
                    .ForMember(x => x.StatusId, m => m.MapFrom(x => (int)(x.Enabled ? MonitorStatus.OK : MonitorStatus.InActive)))
                    ;
            cfg.CreateMap<NcMonitor, MonitorFormViewModel>()
                //.ForMember(x => x.StatusName, m => m.MapFrom(x => x.Status.Name))
                ;
            cfg.CreateMap<NcMonitor, MonitorDetailDTO>();

            cfg.CreateMap<NcMonitorRecord, MonitorRecordListDTO>()
                .ForMember(x => x.GroupBy, m => m.MapFrom(x => x.StartDate.Date));


            cfg.CreateMap<NcScenario, ScenarioDetailViewModel>()
                .ForMember(x => x.Monitors, m => m.Ignore());

            cfg.CreateMap<NcScenario, ScenarioViewModel>();
            cfg.CreateMap<ScenarioViewModel, NcScenario>();

            cfg.CreateMap<NcScenarioItem, ScenarioItemViewModel>();
            cfg.CreateMap<ScenarioItemViewModel, NcScenarioItem>();


            cfg.CreateMap<NcChannel, ChannelViewModel>();
            cfg.CreateMap<ChannelViewModel, NcChannel>();

            cfg.CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(s => s.RoleName, m => m.Ignore());

            cfg.CreateMap<NcMonitorMethodType, MonitorMethodTypeDTO>();

            cfg.CreateMap<NcMonitorVerificationType, MonitorVerificationTypeDTO>();
        }

        private static void MapSelectItem<TSource, TKey>(this IMapperConfigurationExpression mapper, Expression<Func<TSource, TKey>> value, Expression<Func<TSource, string>> text)
        {
            mapper.CreateMap<TSource, ISelectItem<TKey>>()
                .ForMember(x=>x.Value, m=>m.MapFrom(value))
                .ForMember(x => x.Text, m => m.MapFrom(text))
                ;
        }
    }
}
