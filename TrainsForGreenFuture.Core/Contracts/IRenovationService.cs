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

        public IEnumerable<InterrailServiceModel> AllInterrails();
        public IEnumerable<CategoryServiceModel> AllCategories();
    }
}
