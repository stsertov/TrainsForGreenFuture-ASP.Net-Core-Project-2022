namespace TrainsForGreenFuture.Infrastructure
{
    using AutoMapper;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Models.Orders;
    using TrainsForGreenFuture.Infrastructure.Data.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Locomotive, LocomotiveViewModel>()
                .ForMember(lv => lv.Interrail, cfg => cfg.MapFrom(lv => lv.Interrail.Length))
                .ForMember(lv => lv.EngineType, cfg => cfg.MapFrom(lv => lv.EngineType.ToString()));


            CreateMap<Interrail, InterrailServiceModel>();

            CreateMap<Locomotive, LocomotiveFormModel>()
                .ForMember(lf => lf.EngineType, cfg => cfg.MapFrom(l => l.EngineType.ToString()));


            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderType, cfg => cfg.MapFrom(o => o.OrderType.ToString()));

        }
    }
}
