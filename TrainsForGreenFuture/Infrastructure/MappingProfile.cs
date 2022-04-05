namespace TrainsForGreenFuture.Infrastructure
{
    using AutoMapper;
    using TrainsForGreenFuture.Core.Models.Api.Locomotives;
    using TrainsForGreenFuture.Core.Models.Api.TrainCars;
    using TrainsForGreenFuture.Core.Models.Api.Trains;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Core.Models.Trains;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LocomotiveViewModel, LocomotiveFormModel>();

            CreateMap<LocomotiveViewModel, LocomotiveResponseModel>();

            CreateMap<TrainCarViewModel, TrainCarResponseModel>();

            CreateMap<TrainViewModel, TrainResponseModel>();
        }
    }
}
