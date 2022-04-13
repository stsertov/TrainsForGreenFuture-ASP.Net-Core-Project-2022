namespace TrainsForGreenFuture.XUnitTest.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using TrainForGreenFuture.Test;
    using TrainsForGreenFuture.Controllers;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using Xunit;

    public class TrainCarControllerTest
    {
        TrainCarsController controller;
        ServiceProvider serviceProvider;
        InMemoryDbContext dbContext;

        public void Setup()
        {
            dbContext = new InMemoryDbContext();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));

            var serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateDb())
                .AddSingleton<IMapper, Mapper>()
                .AddSingleton<ITrainCarService, TrainCarService>(tcs =>
                new TrainCarService(
                    tcs.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        public void TearDown()
           => dbContext.Dispose();

        [Fact]
        public void AllTrainCarsReturnsRealData()
        {
            Setup();
            //Arrange
            var service = serviceProvider.GetService<ITrainCarService>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            controller = new TrainCarsController(service, cache);

            //Act
            var result = controller.All();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<List<TrainCarViewModel>>(viewResult.Model);
            Assert.NotNull(modelResult);
            Assert.Equal(2, modelResult.Count());

            TearDown();
        }

        [Fact]
        public void DetailsReturnsRealData()
        {
            Setup();
            //Arrange
            var service = serviceProvider.GetService<ITrainCarService>();
            controller = new TrainCarsController(service, Mock.Of<IMemoryCache>());

            //Act
            var result = controller.Details(1);
            var deletedResult = controller.Details(3);
            var renovationResult = controller.Details(4);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(deletedResult);
            Assert.NotNull(renovationResult);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RedirectResult>(deletedResult);
            Assert.IsType<RedirectResult>(renovationResult);

            var modelResult = Assert.IsType<TrainCarViewModel>(viewResult.Model);
            Assert.Equal(1, modelResult.Id);

            TearDown();
        }


        private void SeedData(TrainsDbContext context)
        {
            context.Interrails.AddRange(new[]
            {
                new Interrail { Length = 760 },
                new Interrail { Length = 1435 },
                new Interrail { Length = 1520 }
            });

            context.SaveChanges();

            context.Categories.AddRange(new[]
            {
                new Category { Name = "Passenger" },
                new Category { Name = "Education" },
                new Category { Name = "Sleeper"},
                new Category { Name = "Restaurant"}
            });

            context.SaveChanges();

            context.TrainCars.Add(new TrainCar
            {
                Model = "Sleeper Orwell",
                Year = 1984,
                Series = 58000,
                CategoryId = 3,
                SeatCount = 45,
                LuxuryLevel = (LuxuryLevel)1,
                InterrailId = 2,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Sleeper train car",
                Price = 3500000m,
                IsDeleted = false,
                IsForRenovation = false
            });

            context.TrainCars.Add(new TrainCar
            {
                Model = "Restaurant Orwell",
                Year = 1994,
                Series = 68002,
                CategoryId = 4,
                SeatCount = 45,
                LuxuryLevel = (LuxuryLevel)1,
                InterrailId = 2,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Restaurant train car",
                Price = 2500000m,
                IsDeleted = false,
                IsForRenovation = false
            });

            context.TrainCars.Add(new TrainCar
            {
                Model = "Restaurant Orwell",
                Year = 1994,
                Series = 68002,
                CategoryId = 4,
                SeatCount = 45,
                LuxuryLevel = (LuxuryLevel)1,
                InterrailId = 2,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Restaurant train car",
                Price = 2500000m,
                IsDeleted = true,
                IsForRenovation = false
            });

            context.TrainCars.Add(new TrainCar
            {
                Model = "Restaurant Orwell",
                Year = 1994,
                Series = 68002,
                CategoryId = 4,
                SeatCount = 45,
                LuxuryLevel = (LuxuryLevel)1,
                InterrailId = 2,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Restaurant train car",
                Price = 2500000m,
                IsDeleted = false,
                IsForRenovation = true
            });

            context.SaveChanges();
        }
    }
}
