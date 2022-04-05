
namespace TrainsForGreenFuture.Core.Models.Renovations
{
    using System.Collections.Generic;
    using TrainsForGreenFuture.Core.Models;

    public class AllRenovationsViewModel
    {
        public const int RenovationsPerPage = 4;

        public GlobalSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalRenovations { get; set; }

        public IEnumerable<RenovationViewModel> Renovations { get; set; }
    }
}