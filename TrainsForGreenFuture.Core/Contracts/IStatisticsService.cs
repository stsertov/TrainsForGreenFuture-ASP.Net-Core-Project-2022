namespace TrainsForGreenFuture.Core.Contracts
{
    using TrainsForGreenFuture.Core.Models.Api.Statistics;

    public interface IStatisticsService
    {
        public StatisticsServiceModel GetInfo();
    }
}
