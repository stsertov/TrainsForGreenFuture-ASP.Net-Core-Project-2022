namespace TrainsForGreenFuture.Core.Contracts
{
    using TrainsForGreenFuture.Core.Models.Categories;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public interface ITrainCarService
    {
        public IEnumerable<TrainCarViewModel> AllTrainCars();

        public IEnumerable<InterrailServiceModel> AllInterrails();
        public IEnumerable<CategoryServiceModel> AllCategories();

        public int Create(
            string model,
            int year,
            int series,
            int categoryId,
            int seatCount,
            LuxuryLevel luxuryLevel,
            int interrailId,
            string picture,
            string description,
            decimal price);

        public bool Edit(
            int id,
            string model,
            int year,
            int series,
            int categoryId,
            int seatCount,
            LuxuryLevel luxuryLevel,
            int interrailId,
            string picture,
            string description,
            decimal price);

        public TrainCarViewModel Details(int trainCarId);

        public TrainCarFormModel FormDetails(int trainCarId);
        public string GetCategoryName(int categoryId);
    }
}
