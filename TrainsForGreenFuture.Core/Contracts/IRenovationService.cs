namespace TrainsForGreenFuture.Core.Contracts
{
    using TrainsForGreenFuture.Core.Models;
    using TrainsForGreenFuture.Core.Models.Categories;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Renovations;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public interface IRenovationService
    {
        public AllRenovationsViewModel All(
            string userId,
            GlobalSorting sorting,
            int currentPage,
            int renovationPerPage);
        public AllRenovationsViewModel AllFinished(
            GlobalSorting sorting,
            int currentPage,
            int renovationPerPage);

        public string CreateLocomotiveRenovation(
            string userId,
            RenovationVolume renovationVolume,
            string model,
            int year,
            int series,
            EngineType engineType,
            int interrailId,
            string picture,
            string description);

        public string CreateTrainCarRenovation(
                string userId,
                RenovationVolume renovationVolume,
                string model,
                int year,
                int series,
                int categoryId,
                LuxuryLevel luxuryLevel,
                int interrailId,
                string picture,
                string description);

        public bool CancelRenovation(string id, string comment);

        public RenovationDetailsViewModel Details(string id);

        public AllRenovationsViewModel AllRenovations(
           GlobalSorting sorting = GlobalSorting.DateCreated,
           int currentPage = 1,
           int renovationPerPage = int.MaxValue);

        public IEnumerable<RenovationViewModel> ApiRenovations();
        public IEnumerable<InterrailServiceModel> AllInterrails();
        public IEnumerable<CategoryServiceModel> AllCategories();
    }
}
