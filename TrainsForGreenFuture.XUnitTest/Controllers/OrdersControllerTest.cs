namespace TrainsForGreenFuture.XUnitTest.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System;
    using System.Linq;
    using TrainForGreenFuture.Test;
    using TrainsForGreenFuture.Controllers;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Models.Orders;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using Xunit;

    public class OrdersControllerTest
    {
        OrdersController controller;
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
                .AddSingleton<IOrderService, OrderService>(tcs =>
                new OrderService(
                    tcs.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .AddSingleton<ILocomotiveService, LocomotiveService>(tcs =>
                new LocomotiveService(
                    mapper,
                    tcs.GetRequiredService<TrainsDbContext>()))
                .AddSingleton<ITrainCarService, TrainCarService>(tcs =>
                new TrainCarService(
                    tcs.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .AddSingleton<ITrainService, TrainService>(tcs =>
                new TrainService(
                    tcs.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        public void TearDown()
            => dbContext.Dispose();

        [Fact]
        public void OrderLocomotivesCreatesProperOrder()
        {
            Setup();

            //Arrange
            var service = serviceProvider.GetService<IOrderService>();
            var locomotiveService = serviceProvider.GetService<ILocomotiveService>();
            controller = new OrdersController(
                service,
                locomotiveService,
                Mock.Of<ITrainCarService>(),
                Mock.Of<ITrainService>(),
                Mock.Of<IMapper>());

            //Act
            var result = controller.OrderLocomotive(1);
            var fakeResult = controller.OrderLocomotive(10);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(fakeResult);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RedirectToActionResult>(fakeResult);

            var modelResult = Assert.IsType<OrderLocomotiveFormModel>(viewResult.Model);
            Assert.Equal(1, modelResult.Locomotive.Id);

            TearDown();
        }

        [Fact]
        public void OrderTrainCarsCreatesProperOrder()
        {
            Setup();

            //Arrange
            var service = serviceProvider.GetService<IOrderService>();
            var trainCarService = serviceProvider.GetService<ITrainCarService>();
            controller = new OrdersController(
                service,
                Mock.Of<ILocomotiveService>(),
                trainCarService,
                Mock.Of<ITrainService>(),
                Mock.Of<IMapper>());

            //Act
            var result = controller.OrderTrainCar(1);
            var fakeResult = controller.OrderTrainCar(10);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(fakeResult);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RedirectToActionResult>(fakeResult);

            var modelResult = Assert.IsType<OrderTrainCarFormModel>(viewResult.Model);
            Assert.Equal(1, modelResult.TrainCar.Id);

            TearDown();
        }

        [Fact]
        public void OrderTrainsCreatesProperOrder()
        {
            Setup();

            //Arrange
            var service = serviceProvider.GetService<IOrderService>();
            var trainService = serviceProvider.GetService<ITrainService>();
            controller = new OrdersController(
                service,
                Mock.Of<ILocomotiveService>(),
                Mock.Of<ITrainCarService>(),
                trainService,
                Mock.Of<IMapper>());

            //Act
            var result = controller.OrderTrain(1);
            var fakeResult = controller.OrderTrain(10);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(fakeResult);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RedirectToActionResult>(fakeResult);

            var modelResult = Assert.IsType<OrderTrainFormModel>(viewResult.Model);
            Assert.Equal(1, modelResult.Train.Id);

            TearDown();
        }

        private void SeedData(TrainsDbContext context)
        {
            var interrail = new Interrail { Length = 1435 };
            var category = new Category { Name = "SomeCategory" };
            var user = new User { Id = "userId", FirstName = "user", LastName = "user", Company = "User user", Email = "user@user.com", PasswordHash = "hashedPassword", };
            var locomotive = new Locomotive
            {
                Model = "Lyudmila Orwell",
                Year = 1994,
                Series = 92403,
                EngineType = (EngineType)0,
                Interrail = interrail,
                TopSpeed = 160,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Diesel locomotive",
                Price = 2500000m,
                IsDeleted = false,
                IsForRenovation = false
            };

            var trainCar = new TrainCar
            {
                Model = "Sleeper Orwell",
                Year = 1984,
                Series = 58000,
                Category = category,
                SeatCount = 45,
                LuxuryLevel = (LuxuryLevel)1,
                Interrail = interrail,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Sleeper train car",
                Price = 3500000m,
                IsDeleted = false,
                IsForRenovation = false
            };

            var train = new Train
            {
                Model = "Train Orwell",
                Year = 1984,
                Series = 27403,
                LuxuryLevel = (LuxuryLevel)0,
                TrainCarCount = 5,
                EngineType = (EngineType)1,
                Interrail = interrail,
                TopSpeed = 180,
                Picture = "https://upload.wikimedia.org/wikipedia/commons/3/37/45_204.5_%D1%81%D0%BB%D0%B5%D0%B4_%D0%91%D0%92_4681-06-08-2016%D0%B3-%D0%A6%D0%93_%D0%9F%D0%BE-2.jpg",
                Description = "Good retro train",
                Price = 3500000m,
                IsDeleted = false,
                IsForRenovation = false
            };

            context.Orders.AddRange(new Order[]
                {
                    new Order
                    {
                        Id = "locomotiveOrder",
                        OrderType = (OrderType)0,
                        OrderDate = DateTime.UtcNow,
                        User = user,
                        Locomotive = locomotive,
                        InterrailLength = 1520,
                        AdditionalInterrailTax = 500000m,
                        Count = 3,
                        IsApproved = false,
                        IsPaid = false
                    },
                    new Order
                    {
                        Id = "ApprovedLocomotiveOrder",
                        OrderType = (OrderType)0,
                        OrderDate = DateTime.UtcNow,
                        User = user,
                        Locomotive = locomotive,
                        InterrailLength = 1520,
                        AdditionalInterrailTax = 500000m,
                        Count = 3,
                        IsApproved = true,
                        IsPaid = false
                    },
                    new Order
                    {
                        Id = "trainCarOrder",
                        OrderType = (OrderType)1,
                        OrderDate = DateTime.UtcNow,
                        User = user,
                        TrainCar = trainCar,
                        InterrailLength = 1435,
                        LuxuryLevel = (LuxuryLevel)0,
                        AdditionalLuxuryLevelTax = 200000m,
                        Count = 3,
                        IsApproved = false,
                        IsPaid = false
                    },
                    new Order
                    {
                        Id = "trainOrder",
                        OrderType = (OrderType)2,
                        OrderDate = DateTime.UtcNow,
                        User = user,
                        Train = train,
                        InterrailLength = 1435,
                        LuxuryLevel = (LuxuryLevel)0,
                        AdditionalLuxuryLevelTax = 200000m,
                        Count = 3,
                        IsApproved = false,
                        IsPaid = false
                    }
            });

            context.SaveChanges();
        }
    }
}
