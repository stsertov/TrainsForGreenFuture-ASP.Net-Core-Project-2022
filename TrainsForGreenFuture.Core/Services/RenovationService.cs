namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models;
    using TrainsForGreenFuture.Core.Models.Renovations;
    using TrainsForGreenFuture.Infrastructure.Data;

    public class RenovationService : IRenovationService
    {
        private TrainsDbContext context;
        private IMapper mapper;
        public RenovationService(TrainsDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public AllRenovationsViewModel All(
            GlobalSorting sorting = GlobalSorting.DateCreated,
            int currentPage = 1,
            int renovationPerPage = int.MaxValue)
        {
            var dbRenovations = context.Renovations.AsQueryable();

            dbRenovations = sorting switch
            {
                GlobalSorting.Status => dbRenovations.OrderByDescending(r => r.IsApproved).ThenByDescending(r => r.IsPaid),
                GlobalSorting.Type => dbRenovations.OrderBy(r => r.RenovationType),
                GlobalSorting.DateCreated or _ => dbRenovations.OrderByDescending(r => r.DateCreated)
            };

            var totalRenovations = dbRenovations.Count();

            var renovations = dbRenovations
                .Skip((currentPage - 1) * renovationPerPage)
                .Take(renovationPerPage)
                .ToList();

            return new AllRenovationsViewModel
            {
                TotalRenovations = totalRenovations,
                Sorting = sorting,
                Renovations = mapper.Map<List<RenovationViewModel>>(renovations)
            };
        }
    }
}
