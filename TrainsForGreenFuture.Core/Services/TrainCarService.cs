namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data;

    public class TrainCarService : ITrainCarService
    {
        private TrainsDbContext context;
        private IMapper mapper;
        public TrainCarService(TrainsDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<TrainCarViewModel> AllTrainCars()
        {
            var trainCars = context.TrainCars.ToList();

            return mapper.Map<List<TrainCarViewModel>>(trainCars);
        }
    }
}
