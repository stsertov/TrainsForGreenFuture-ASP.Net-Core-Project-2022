namespace TrainsForGreenFuture.Core.Services
{
    using System.Linq;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Api.Statistics;
    using TrainsForGreenFuture.Infrastructure.Data;

    public class StatisticsService : IStatisticsService
    {
        private TrainsDbContext context;

        public StatisticsService(TrainsDbContext context)
            => this.context = context;

        public StatisticsServiceModel GetInfo()
            => new StatisticsServiceModel
            {
                LocomotivesCount = LocomotivesCount(),
                SoldLocomotivesCount = SoldLocomotivesCount(),
                TrainCarsCount = TrainCarsCount(),
                SoldTrainCarsCount = SoldTrainCarsCount(),
                TrainsCount = TrainsCount(),
                SoldTrainsCount = SoldTrainsCount()
            };

        private int LocomotivesCount()
            => context.Locomotives.Count(l => !l.IsForRenovation && !l.IsDeleted);

        private int SoldLocomotivesCount()
            => context.Orders
            .Where(l => l.IsApproved && l.IsPaid)
            .ToArray()
            .Where(l => l.OrderType.ToString() == "Locomotive")
            .Sum(l => l.Count);


        private int TrainCarsCount()
            => context.TrainCars.Count(tc => !tc.IsForRenovation && !tc.IsDeleted);

        private int SoldTrainCarsCount()
            => context.Orders
            .Where(l => l.IsApproved && l.IsPaid)
            .ToArray()
            .Where(tc => tc.OrderType.ToString() == "TrainCar")
            .Sum(tc => tc.Count);


        private int TrainsCount()
           => context.Trains.Count(t => !t.IsForRenovation && !t.IsDeleted);

        private int SoldTrainsCount()
            => context.Orders
            .Where(l => l.IsApproved && l.IsPaid)
            .ToArray()
            .Where(t => t.OrderType.ToString() == "Train")
            .Sum(t => t.Count);
    }
}
