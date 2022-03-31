namespace TrainsForGreenFuture.Core.Contracts
{
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public interface ILocomotiveService
    {
        public IEnumerable<LocomotiveViewModel> AllLocomotives();

        public IEnumerable<InterrailServiceModel> AllInterrails();

        public int Create(
            string model, 
            int year, 
            int series, 
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
            EngineType engineType, 
            int interrailId, 
            int topSpeed, 
            string picture, 
            string description, 
            decimal price);

        public LocomotiveViewModel Details(int id);

        public bool Remove(int id);
    }
}
