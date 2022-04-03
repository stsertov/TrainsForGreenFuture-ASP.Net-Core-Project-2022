namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Trains;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

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

        public int Create(
            string model, 
            int year, 
            int series, 
            LuxuryLevel luxuryLevel, 
            int trainCarCount, 
            EngineType engineType,
            int interrailId, 
            int topSpeed, 
            string picture,
            string description,
            decimal price)
        {
            var train = new Train()
            {
                Model = model,
                Year = year,
                Series = series,
                LuxuryLevel = luxuryLevel,                              
                TrainCarCount = trainCarCount,
                EngineType = engineType,
                InterrailId = interrailId,
                TopSpeed = topSpeed,
                Picture = picture,
                Description = description,
                Price = price
            };

            context.Trains.Add(train);
            context.SaveChanges();

            return train.Id;
        }

        public bool Edit(
            int id,
            string model,
            int year,
            int series,
            LuxuryLevel luxuryLevel,
            int trainCarCount,
            EngineType engineType,
            int interrailId,
            int topSpeed,
            string picture,
            string description,
            decimal price)
        {
            var dbTrain = GetTrain(id);

            if (dbTrain == null)
            {
                return false;
            }

            dbTrain.Model = model;
            dbTrain.Year = year;
            dbTrain.Series = series;
            dbTrain.LuxuryLevel = luxuryLevel;
            dbTrain.TrainCarCount = trainCarCount;
            dbTrain.EngineType = engineType;
            dbTrain.InterrailId = interrailId;
            dbTrain.TopSpeed = topSpeed;
            dbTrain.Picture = picture;
            dbTrain.Description = description;
            dbTrain.Price = price;

            context.SaveChanges();

            return true;
        }

        public TrainViewModel Details(int trainId)
        {
            var dbTrain = GetTrain(trainId);

            if (dbTrain == null)
            {
                return null;
            }

            var train = mapper.Map<TrainViewModel>(dbTrain);

            return train;
        }

        public TrainFormModel FormDetails(int trainId)
        {
            var dbTrain = GetTrain(trainId);

            if (dbTrain == null)
            {
                return null;
            }

            var train = mapper.Map<TrainFormModel>(dbTrain);

            return train;
        }
        public bool Remove(int id)
        {
            var train = GetTrain(id);

            if (train == null)
            {
                return false;
            }

            train.IsDeleted = true;
            context.SaveChanges();

            return true;
        }

        public IEnumerable<InterrailServiceModel> AllInterrails()
        {
            var interrails = context.Interrails.ToList();

            return mapper.Map<List<InterrailServiceModel>>(interrails);
        }

        private Train GetTrain(int id)
            => context.Trains
                .Include(t => t.Interrail)
                .FirstOrDefault(t => !t.IsForRenovation && !t.IsDeleted && t.Id == id);

        
    }
}
