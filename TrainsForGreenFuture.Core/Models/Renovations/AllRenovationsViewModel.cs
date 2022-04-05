
namespace TrainsForGreenFuture.Core.Models.Renovations
{
    using System.Collections.Generic;
    using TrainsForGreenFuture.Core.Models;

    public class AllRenovationsViewModel
    {
        public const int RenovationsPerPage = 5;

        public GlobalSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalRenovations { get; set; }

        public IEnumerable<RenovationViewModel> Renovations { get; set; }
    }
}