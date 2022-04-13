namespace TrainsForGreenFuture.Test.Controllers
{
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using TrainForGreenFuture.Test;
    using TrainsForGreenFuture.Controllers;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Models.Home;
    using Xunit;

    public class HomeControllerTest
    {
        HomeController controller;
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
                .AddSingleton<ITrainCarService, TrainCarService>(ts => 
                new TrainCarService(
                    ts.GetRequiredService<TrainsDbContext>(),
                mapper))
                .AddSingleton<ITrainService, TrainService>(ts =>
                new TrainService(
                    ts.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
        }

        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Fact]
        public void IndexReturnsViewResult()
        {
            //Arrange
            Setup();
            controller = new HomeController(
                Mock.Of<ILocomotiveService>(), 
                Mock.Of<ITrainCarService>(), 
                Mock.Of<ITrainService>());

            //Act
            var result = controller.Index();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            TearDown();
        }

        [Fact]
        public void TrainsReturnViewResultWithListOfTrains()
        {
            //Arrange
            Setup();
            controller = new HomeController(
                Mock.Of<ILocomotiveService>(),
                Mock.Of<ITrainCarService>(),
                Mock.Of<ITrainService>());

            //Act
            var result = controller.Trains();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var modelResult = Assert.IsType<List<TrainsGenericViewModel>>(viewResult.Model);
            Assert.Equal(3, modelResult.Count());
            TearDown();
        }
    }
}
