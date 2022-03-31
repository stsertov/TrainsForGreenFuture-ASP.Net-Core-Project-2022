namespace TrainsForGreenFuture.Infrastructure
{
    using AutoMapper;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Models.Orders;
    using TrainsForGreenFuture.Core.Models.TrainCars;
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
                .ForMember(o => o.OrderType, cfg => cfg.MapFrom(o => o.OrderType.ToString()))
                .ForMember(o => o.Company, cfg => cfg.MapFrom(o => o.User.Company));


            CreateMap<TrainCar, TrainCarViewModel>()
                .ForMember(tc => tc.CategoryName, cfg => cfg.MapFrom(t => t.Category.Name))
                .ForMember(tc => tc.LuxuryLevel, cfg => cfg.MapFrom(t => t.LuxuryLevel == null ? null : t.LuxuryLevel.ToString()))
                .ForMember(tc => tc.CargoType, cfg => cfg.MapFrom(t => t.CargoType == null ? null : t.CargoType.ToString()))
                .ForMember(tc => tc.InterraiLength, cfg => cfg.MapFrom(t => t.Interrail.Length));
        }
    }
}
