namespace TrainsForGreenFuture.Core.Contracts
{
    using System.Collections.Generic;
    using TrainsForGreenFuture.Core.Models.Trains;

    public interface ITrainService
    {
        public IEnumerable<TrainViewModel> AllTrains();

        public TrainViewModel Details(int trainId);
    }
}
