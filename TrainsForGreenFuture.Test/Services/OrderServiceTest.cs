namespace TrainsForGreenFuture.Test.Services
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TrainForGreenFuture.Test;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class OrderServiceTest
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
                .AddSingleton<IOrderService, OrderService>(os =>
                new OrderService(
                    os.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        [Test]

        public void AllOrdersCountForAdminArea()
        {
            //Arrange
            var service = serviceProvider.GetService<IOrderService>();

            //Act
            var result = service.All().Count();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void AllOrdersByUser()
        {
            //Arrange
            var service = serviceProvider.GetService<IOrderService>();

            //Act
            var result = service.All("userId").Count();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(3, result);
        }

        [Test]

        public void CreateLocomotiveOrderCreatesProperOrder()
        {
            //Arrange
            var service = serviceProvider.GetService<IOrderService>();

            //Act
            var result = service.CreateLocomotiveOrder("userId", 1, 1435, 500000m, 3);
            var ordersCount = service.All().Count();
            var isPresent = service.All().Any(o => o.Id == result);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(4, ordersCount);
            Assert.That(isPresent);
        }

        [Test]

        public void CreateTrainCarOrderCreatesProperOrder()
        {
            //Arrange
            var service = serviceProvider.GetService<IOrderService>();

            //Act
            var result = service.CreateTrainCarOrder("userId", 1, 1435, 500000m, (LuxuryLevel)0, 200000m, 3);
            var ordersCount = service.All().Count();
            var isPresent = service.All().Any(o => o.Id == result);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(4, ordersCount);
            Assert.That(isPresent);
        }

        [Test]

        public void CreateTrainOrderCreatesProperOrder()
        {
            //Arrange
            var service = serviceProvider.GetService<IOrderService>();

            //Act
            var result = service.CreateTrainCarOrder("userId", 1, 1435, 500000m, (LuxuryLevel)0, 200000m, 3);
            var ordersCount = service.All().Count();
            var isPresent = service.All().Any(o => o.Id == result);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(4, ordersCount);
            Assert.That(isPresent);
        }

        [Test]
        public void ChangingStatusOfExistingOrdersOnly()
        {
            //Arrange
            var service = serviceProvider.GetService<IOrderService>();

            //Act
            var success = service.ChangeStatus("locomotiveOrder");
            var fail = service.ChangeStatus("notRealId");
            var statusChanged = service.All().Any(o => o.IsApproved);

            //Assert
            Assert.IsTrue(success);
            Assert.IsFalse(fail);
            Assert.IsTrue(statusChanged);
        }

        [Test]
        public void ChangingThePaidStatusOnlyOfTheApprovedOrders()
        {
            //Arrange
            var service = serviceProvider.GetService<IOrderService>();

            //Act
            service.ChangeStatus("locomotiveOrder");
            var success = service.ChangePaidStatus("locomotiveOrder");
            var existentFail = service.ChangePaidStatus("trainCarOrder");
            var nonExistentFail = service.ChangePaidStatus("notRealId");
            var statusChanged = service.All().Any(o => o.IsPaid);

            //Assert
            Assert.IsTrue(success);
            Assert.IsFalse(existentFail);
            Assert.IsFalse(nonExistentFail);
            Assert.IsTrue(statusChanged);
        }

        [TearDown]
        public void TearDown()
           => dbContext.Dispose();

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
