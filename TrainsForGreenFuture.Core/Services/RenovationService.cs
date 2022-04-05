namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models;
    using TrainsForGreenFuture.Core.Models.Categories;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Renovations;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

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
            string userId,
            GlobalSorting sorting = GlobalSorting.DateCreated,
            int currentPage = 1,
            int renovationPerPage = int.MaxValue)
        {
            var dbRenovations = context.Renovations
                .Include(r => r.Locomotive)
                .ThenInclude(l => l.Interrail)
                .Include(r => r.TrainCar)
                .ThenInclude(tc => tc.Category)
                .Include(r => r.TrainCar)
                .ThenInclude(tc => tc.Interrail)
                .Where(r => r.UserId == userId)
                .AsQueryable();

            return GetRenovation(dbRenovations, sorting, currentPage, renovationPerPage);
        }

        public AllRenovationsViewModel AllFinished(
            GlobalSorting sorting = GlobalSorting.DateCreated,
            int currentPage = 1,
            int renovationPerPage = int.MaxValue)
        {
            var dbRenovations = context.Renovations
                .Include(r => r.Locomotive)
                .ThenInclude(l => l.Interrail)
                .Include(r => r.TrainCar)
                .ThenInclude(tc => tc.Category)
                .Include(r => r.TrainCar)
                .ThenInclude(tc => tc.Interrail)
                .Where(r => r.IsPaid)
                .AsQueryable();

            return GetRenovation(dbRenovations, sorting, currentPage, renovationPerPage);
        }

        public string CreateLocomotiveRenovation(
            string userId,
            RenovationVolume renovationVolume,
            string model,
            int year,
            int series,
            EngineType engineType,
            int interrailId,
            string picture,
            string description)
        {
            var locomotive = new Locomotive
            {
                Model = model,
                Year = year,
                Series = series,
                EngineType = engineType,
                InterrailId = interrailId,
                Picture = picture,
                Description = description,
                IsForRenovation = true
            };

            context.Locomotives.Add(locomotive);
            context.SaveChanges();

            var renovation = new Renovation
            {
                RenovationVolume = renovationVolume,
                RenovationType = RenovationType.Locomotive,
                DateCreated = DateTime.UtcNow,
                LocomotiveId = locomotive.Id,
                UserId = userId
            };

            context.Renovations.Add(renovation);

            context.SaveChanges();

            return renovation.Id;
        }

        public IEnumerable<InterrailServiceModel> AllInterrails()
            => mapper.Map<List<InterrailServiceModel>>(context.Interrails.ToArray());

        public IEnumerable<CategoryServiceModel> AllCategories()
            => mapper.Map<List<CategoryServiceModel>>(context.Categories.ToArray());


        private AllRenovationsViewModel GetRenovation(
            IQueryable<Renovation> query,
            GlobalSorting sorting,
            int currentPage,
            int renovationPerPage)
        {
            query = sorting switch
            {
                GlobalSorting.Status => query.OrderByDescending(r => r.IsApproved).ThenByDescending(r => r.IsPaid),
                GlobalSorting.Type => query.OrderBy(r => r.RenovationType),
                GlobalSorting.DateCreated or _ => query.OrderByDescending(r => r.DateCreated)
            };

            var totalRenovations = query.Count();

            var renovations = query
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
