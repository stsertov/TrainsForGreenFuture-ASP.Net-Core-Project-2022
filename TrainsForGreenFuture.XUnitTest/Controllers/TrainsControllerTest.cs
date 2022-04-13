namespace TrainsForGreenFuture.XUnitTest.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TrainForGreenFuture.Test;
    using TrainsForGreenFuture.Controllers;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Models.Trains;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using Xunit;

    public class TrainsControllerTest
    {
        TrainsController controller;
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
                .AddSingleton<ITrainService, TrainService>(ts =>
                new TrainService(
                    ts.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        public void TearDown()
           => dbContext.Dispose();

        [Fact]
        public void AllTrainsReturnsRealData()
        {
            Setup();
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            controller = new TrainsController(service, cache);

            //Act
            var result = controller.All();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<List<TrainViewModel>>(viewResult.Model);
            Assert.NotNull(modelResult);
            Assert.Equal(2, modelResult.Count());

            TearDown();
        }

        [Fact]
        public void DetailsReturnsRealData()
        {
            Setup();
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();
            controller = new TrainsController(service, Mock.Of<IMemoryCache>());

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

            var modelResult = Assert.IsType<TrainViewModel>(viewResult.Model);
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

            context.Trains.Add(new Train
            {
                Model = "Train Orwell",
                Year = 1984,
                Series = 27403,
                LuxuryLevel = (LuxuryLevel)0,
                TrainCarCount = 5,
                EngineType = (EngineType)1,
                InterrailId = 2,
                TopSpeed = 180,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Good retro train",
                Price = 3500000m,
                IsDeleted = false,
                IsForRenovation = false
            });

            context.Trains.Add(new Train
            {
                Model = "Train Douglas",
                Year = 1994,
                Series = 28403,
                LuxuryLevel = (LuxuryLevel)0,
                TrainCarCount = 5,
                EngineType = (EngineType)1,
                InterrailId = 2,
                TopSpeed = 180,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Good retro train",
                Price = 3500000m,
                IsDeleted = false,
                IsForRenovation = false
            });

            context.Trains.Add(new Train
            {
                Model = "Deleted Orwell",
                Year = 1994,
                Series = 30403,
                LuxuryLevel = (LuxuryLevel)0,
                TrainCarCount = 5,
                EngineType = (EngineType)1,
                InterrailId = 2,
                TopSpeed = 180,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Good retro train",
                Price = 3500000m,
                IsDeleted = true,
                IsForRenovation = false
            });

            context.Trains.Add(new Train
            {
                Model = "Renovated Orwell",
                Year = 1994,
                Series = 25403,
                LuxuryLevel = (LuxuryLevel)0,
                TrainCarCount = 5,
                EngineType = (EngineType)1,
                InterrailId = 2,
                TopSpeed = 180,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Good retro train",
                Price = 3500000m,
                IsDeleted = false,
                IsForRenovation = true
            });

            context.SaveChanges();
        }
    }
}
