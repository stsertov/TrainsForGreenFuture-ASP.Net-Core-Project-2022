namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Trains;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;

    public class TrainService : ITrainService
    {
        private TrainsDbContext context;
        private IMapper mapper;

        public TrainService(TrainsDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<TrainViewModel> AllTrains()
        {
            var dbTrains = context.Trains
                .Include(t => t.Interrail)
                .Where(t => !t.IsForRenovation && !t.IsDeleted)
                .ToList();

            var trains = mapper.Map<IEnumerable<TrainViewModel>>(dbTrains);

            return trains;
        }

        public TrainViewModel Details(int trainId)
        {
            var dbTrain = GetTrain(trainId);

            if (dbTrain == null)
            {
                return null;
            }

            var locomotive = mapper.Map<TrainViewModel>(dbTrain);

            return locomotive;
        }

        private Train GetTrain(int id)
            => context.Trains
                .Include(t => t.Interrail)
                .FirstOrDefault(t => !t.IsForRenovation && !t.IsDeleted && t.Id == id);
    }
}
