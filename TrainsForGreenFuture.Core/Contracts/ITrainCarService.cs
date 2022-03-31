namespace TrainsForGreenFuture.Core.Contracts
{
    using TrainsForGreenFuture.Core.Models.TrainCars;

    public interface ITrainCarService
    {
        public IEnumerable<TrainCarViewModel> AllTrainCars();
    }
}
