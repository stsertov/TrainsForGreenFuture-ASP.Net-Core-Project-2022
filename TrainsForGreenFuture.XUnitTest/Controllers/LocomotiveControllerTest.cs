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
    using TrainForGreenFuture.Test;
    using TrainsForGreenFuture.Controllers;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using Xunit;

    public class LocomotiveControllerTest
    {
        LocomotivesController controller;
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
                .AddSingleton<ILocomotiveService, LocomotiveService>(ls =>
                new LocomotiveService(
                    mapper,
                    ls.GetRequiredService<TrainsDbContext>()))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        public void TearDown()
            => dbContext.Dispose();
        

        [Fact]
        public void AllLocomotivesReturnsProperViewResult()
        {
            Setup();
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            controller = new LocomotivesController(service, cache);

            //Act
            var result = controller.All();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var modelResult = Assert.IsType<List<LocomotiveViewModel>>(viewResult.Model);
            Assert.Equal(2, modelResult.Count());

            TearDown();
        }

        [Fact]
        public void DetailsReturnsRealData()
        {
            Setup();
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();
            controller = new LocomotivesController(service, Mock.Of<IMemoryCache>());

            //Act
            var realResult = controller.Details(1);
            var deletedResult = controller.Details(3);
            var reonvationResult = controller.Details(4);

            //Assert
            Assert.NotNull(realResult);
            Assert.NotNull(deletedResult);
            Assert.NotNull(reonvationResult);

            var viewResult = Assert.IsType<ViewResult>(realResult);
            Assert.IsType<RedirectResult>(deletedResult);
            Assert.IsType<RedirectResult>(reonvationResult);

            var modelResult = Assert.IsType<LocomotiveViewModel>(viewResult.Model);
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

            context.Locomotives.Add(new Locomotive
            {
                Id = 1,
                Model = "Skoda Orwell",
                Year = 1984,
                Series = 82403,
                EngineType = Enum.Parse<EngineType>("Electric"),
                InterrailId = 2,
                TopSpeed = 180,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Old school locomotive",
                Price = 3500000m,
                IsDeleted = false,
                IsForRenovation = false
            });

            context.Locomotives.Add(new Locomotive
            {
                Id = 2,
                Model = "Lyudmila Orwell",
                Year = 1994,
                Series = 92403,
                EngineType = Enum.Parse<EngineType>("Diesel"),
                InterrailId = 1,
                TopSpeed = 160,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Diesel locomotive",
                Price = 2500000m,
                IsDeleted = false,
                IsForRenovation = false
            });

            context.Locomotives.Add(new Locomotive
            {
                Id = 3,
                Model = "Deleted Orwell",
                Year = 1994,
                Series = 92403,
                EngineType = Enum.Parse<EngineType>("Diesel"),
                InterrailId = 1,
                TopSpeed = 160,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Diesel locomotive",
                Price = 2500000m,
                IsDeleted = true,
                IsForRenovation = false
            });

            context.Locomotives.Add(new Locomotive
            {
                Id = 4,
                Model = "Renovated Orwell",
                Year = 1994,
                Series = 92403,
                EngineType = Enum.Parse<EngineType>("Diesel"),
                InterrailId = 1,
                TopSpeed = 160,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Diesel locomotive",
                Price = 2500000m,
                IsDeleted = false,
                IsForRenovation = true
            });

            context.SaveChanges();
        }
    }
}
