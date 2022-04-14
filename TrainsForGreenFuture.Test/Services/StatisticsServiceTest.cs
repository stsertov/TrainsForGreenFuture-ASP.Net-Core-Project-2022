namespace TrainsForGreenFuture.Test.Services
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using TrainForGreenFuture.Test;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class LocomotiveServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            dbContext = new InMemoryDbContext();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));

            var serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateDb())
                .AddSingleton<IMapper, Mapper>()
                .AddSingleton<IStatisticsService, StatisticsService>(sc =>
                new StatisticsService(
                    sc.GetRequiredService<TrainsDbContext>()))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        [TearDown]
        public void TearDown()
            => dbContext.Dispose();

        [Test]
        public void GetInfoReturnsProperData()
        {
            //Arrange
            var service = serviceProvider.GetService<IStatisticsService>();

            //Act
            var result = service.GetInfo();

            //Assert
            Assert.NotNull(result);

            Assert.AreEqual(1, result.LocomotivesCount);
            Assert.AreEqual(1, result.TrainCarsCount);
            Assert.AreEqual(1, result.TrainsCount);
            Assert.AreEqual(0, result.SoldLocomotivesCount);
            Assert.AreEqual(0, result.SoldTrainCarsCount);
            Assert.AreEqual(3, result.SoldTrainsCount);
        }
        private void SeedData(TrainsDbContext context)
        {
            context.Interrails.AddRange(new[]
            {
                new Interrail { Length = 760 },
                new Interrail { Length = 1435 },
                new Interrail { Length = 1520 }
            });

            var user = new User
            {
                Id = "userId",
                FirstName = "user",
                LastName = "user",
                Company = "User user",
                Email = "user@user.com",
                PasswordHash = "hashedPassword",
            };

            context.SaveChanges();

            context.Locomotives.Add(new Locomotive
            {
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
                IsForRenovation = true
            });

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

            context.Orders.AddRange(new Order[]
               {
                    new Order
                    {
                         Id = "locomotiveOrder",
                        OrderType = (OrderType)0,
                        OrderDate = DateTime.UtcNow,
                        User = user,
                        LocomotiveId = 1,
                        InterrailLength = 1520,
                        AdditionalInterrailTax = 500000m,
                        Count = 3,
                        IsApproved = false,
                        IsPaid = true
                    },
                    new Order
                    {
                        Id = "trainCarOrder",
                        OrderType = (OrderType)1,
                        OrderDate = DateTime.UtcNow,
                        User = user,
                        TrainCarId = 1,
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
                        TrainId = 1,
                        InterrailLength = 1435,
                        LuxuryLevel = (LuxuryLevel)0,
                        AdditionalLuxuryLevelTax = 200000m,
                        Count = 3,
                        IsApproved = true,
                        IsPaid = true
                    }
           });

            context.SaveChanges();
        }
    }
}
