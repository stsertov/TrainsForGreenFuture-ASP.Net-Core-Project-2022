namespace TrainsForGreenFuture.Infrastructure
{
    using AutoMapper;
    using TrainsForGreenFuture.Core.Models.Locomotives;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LocomotiveViewModel, LocomotiveFormModel>();
        }
    }
}
