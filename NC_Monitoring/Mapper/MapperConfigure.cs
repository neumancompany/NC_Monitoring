using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NC.AspNetCore.Interfaces;
using NC_Monitoring.Business.DTO;
using NC_Monitoring.Data.Enums;
using NC_Monitoring.Data.Models;
using NC_Monitoring.ViewModels;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NC_Monitoring.Mapper
{
    public static class MapperConfigure
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<NcChannel, ChannelViewModel>();
                cfg.CreateMap<ChannelViewModel, NcChannel>();
            });
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
