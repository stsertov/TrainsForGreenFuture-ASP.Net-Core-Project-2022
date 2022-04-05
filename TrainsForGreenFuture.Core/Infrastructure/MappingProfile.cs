namespace TrainsForGreenFuture.Infrastructure
{
    using AutoMapper;
    using TrainsForGreenFuture.Core.Models.Categories;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Models.Orders;
    using TrainsForGreenFuture.Core.Models.Renovations;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Core.Models.Trains;
    using TrainsForGreenFuture.Infrastructure.Data.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Locomotive, LocomotiveViewModel>()
                .ForMember(lv => lv.Interrail, cfg => cfg.MapFrom(lv => lv.Interrail.Length))
                .ForMember(lv => lv.EngineType, cfg => cfg.MapFrom(lv => lv.EngineType.ToString()));


            CreateMap<Interrail, InterrailServiceModel>();

            CreateMap<Category, CategoryServiceModel>();

            CreateMap<Locomotive, LocomotiveFormModel>()
                .ForMember(lf => lf.EngineType, cfg => cfg.MapFrom(l => l.EngineType.ToString()))
                .ForMember(lf => lf.InterrailId, cfg => cfg.MapFrom(l => l.Interrail.Id));


            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.Company, cfg => cfg.MapFrom(o => o.User.Company));


            CreateMap<TrainCar, TrainCarViewModel>()
                .ForMember(tc => tc.CategoryName, cfg => cfg.MapFrom(t => t.Category.Name))
                .ForMember(tc => tc.LuxuryLevel, cfg => cfg.MapFrom(t => t.LuxuryLevel == null ? null : t.LuxuryLevel.ToString()))
                .ForMember(tc => tc.InterraiLength, cfg => cfg.MapFrom(t => t.Interrail.Length));

            CreateMap<TrainCar, TrainCarFormModel>()
                .ForMember(tcf => tcf.LuxuryLevel, cfg => cfg.MapFrom(tc => tc.LuxuryLevel.ToString()));

            CreateMap<Train, TrainViewModel>()
                .ForMember(tv => tv.LuxuryLevel, cfg => cfg.MapFrom(t => t.LuxuryLevel.ToString()))
                .ForMember(tv => tv.EngineType, cfg => cfg.MapFrom(t => t.EngineType.ToString()));

            CreateMap<Train, TrainFormModel>()
                .ForMember(tf => tf.LuxuryLevel, cfg => cfg.MapFrom(t => t.LuxuryLevel.ToString()))
                .ForMember(tf => tf.EngineType, cfg => cfg.MapFrom(t => t.EngineType.ToString()));

            CreateMap<Renovation, RenovationViewModel>();

            CreateMap<Renovation, RenovationDetailsViewModel>();
        }
    }
}
