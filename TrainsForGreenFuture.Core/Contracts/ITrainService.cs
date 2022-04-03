namespace TrainsForGreenFuture.Core.Contracts
{
    using System.Collections.Generic;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Trains;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public interface ITrainService
    {
        public IEnumerable<TrainViewModel> AllTrains();

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
            decimal price);

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
            decimal price);

        public TrainViewModel Details(int trainId);

        public TrainFormModel FormDetails(int trainId);

        public bool Remove(int id);
        public IEnumerable<InterrailServiceModel> AllInterrails();
    }
}
