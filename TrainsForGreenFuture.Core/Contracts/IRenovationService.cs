namespace TrainsForGreenFuture.Core.Contracts
{
    using TrainsForGreenFuture.Core.Models;
    using TrainsForGreenFuture.Core.Models.Renovations;

    public interface IRenovationService
    {

        public AllRenovationsViewModel All(
            GlobalSorting sorting,
            int currentPage,
            int carsPerPage);
    }
}
