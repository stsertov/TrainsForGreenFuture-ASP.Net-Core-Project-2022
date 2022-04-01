namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Categories;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class TrainCarService : ITrainCarService
    {
        private TrainsDbContext context;
        private IMapper mapper;
        public TrainCarService(TrainsDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<TrainCarViewModel> AllTrainCars()
        {
            var trainCars = context.TrainCars
                .Include(tc => tc.Category)
                .Include(tc => tc.Interrail)
                .ToList();

            return mapper.Map<List<TrainCarViewModel>>(trainCars);
        }

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
            decimal price)
        {
            var trainCar = new TrainCar()
            {
                Model = model,
                Year = year,
                Series = series,
                CategoryId = categoryId,
                SeatCount = seatCount,
                LuxuryLevel = luxuryLevel,
                InterrailId = interrailId,
                Picture = picture,
                Description = description,
                Price = price
            };

            context.TrainCars.Add(trainCar);
            context.SaveChanges();

            return trainCar.Id;
        }

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
            decimal price)
        {
            var dbTrainCar = GetTrainCar(id);

            if (dbTrainCar == null)
            {
                return false;
            }

            dbTrainCar.Model = model;
            dbTrainCar.Year = year;
            dbTrainCar.Series = series;
            dbTrainCar.CategoryId = categoryId;
            dbTrainCar.SeatCount = seatCount;
            dbTrainCar.LuxuryLevel = luxuryLevel;
            dbTrainCar.InterrailId = interrailId;
            dbTrainCar.Picture = picture;
            dbTrainCar.Description = description;
            dbTrainCar.Price = price;

            context.SaveChanges();

            return true;
        }
        public TrainCarViewModel Details(int trainCarId)
        {
            var dbTrainCar = GetTrainCar(trainCarId);

            if (dbTrainCar == null)
            {
                return null;
            }

            var trainCar = mapper.Map<TrainCarViewModel>(dbTrainCar);

            return trainCar;
        }
        public TrainCarFormModel FormDetails(int trainCarId)
        {
            var dbTrainCar = GetTrainCar(trainCarId);

            if (dbTrainCar == null)
            {
                return null;
            }

            var trainCar = mapper.Map<TrainCarFormModel>(dbTrainCar);

            return trainCar;
        }

        public bool Remove(int id)
        {
            var trainCar = GetTrainCar(id);

            if (trainCar == null)
            {
                return false;
            }

            context.TrainCars.Remove(trainCar);
            context.SaveChanges();

            return true;
        }

        public IEnumerable<InterrailServiceModel> AllInterrails()
        {
            var interrails = context.Interrails.ToList();

            return mapper.Map<List<InterrailServiceModel>>(interrails);
        }

        public IEnumerable<CategoryServiceModel> AllCategories()
        {
            var categories = context.Categories.ToList();

            return mapper.Map<List<CategoryServiceModel>>(categories);
        }

        public string GetCategoryName(int categoryId)
            => context.Categories
            .Where(c => c.Id == categoryId)
            .Select(c => c.Name)
            .FirstOrDefault();

        private TrainCar GetTrainCar(int trainCarId)
            => context.TrainCars
            .Include(tc => tc.Interrail)
            .Include(tc => tc.Category)
            .FirstOrDefault(tc => tc.Id == trainCarId);
    }
}
