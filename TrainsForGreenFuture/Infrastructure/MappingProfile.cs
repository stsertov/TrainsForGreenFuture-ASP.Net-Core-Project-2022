﻿namespace TrainsForGreenFuture.Infrastructure
{
    using AutoMapper;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Models.Locomotives;
    using TrainsForGreenFuture.Models.Orders;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Locomotive, LocomotiveViewModel>()
                .ForMember(lv => lv.Interrail, cfg => cfg.MapFrom(lv => lv.Interrail.Length))
                .ForMember(lv => lv.EngineType, cfg => cfg.MapFrom(lv => lv.EngineType.ToString()));

            CreateMap<Locomotive, LocomotiveFormModel>()
                .ForMember(lf => lf.EngineType, cfg => cfg.MapFrom(l => l.EngineType.ToString()));


            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderType, cfg => cfg.MapFrom(o => o.OrderType.ToString())); 
        }
    }
}
