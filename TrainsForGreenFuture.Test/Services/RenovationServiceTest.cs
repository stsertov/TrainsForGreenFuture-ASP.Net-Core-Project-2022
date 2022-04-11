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
    using TrainsForGreenFuture.Core.Models;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class RenovationServiceTest
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
                .AddSingleton<IRenovationService, RenovationService>(os =>
                new RenovationService(
                    os.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        [Test]
        public void AllReturnsAllRenovationsByCurrentUser()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var result = service.All("userId", (GlobalSorting)0, 1, 5);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(4, result.Renovations.Count());
        }

        [Test]
        public void GivesBackAllFinishedRenovations()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var result = service.AllFinished((GlobalSorting)0, 1, 5);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Renovations.Count());
            Assert.IsTrue(result.Renovations.FirstOrDefault().IsPaid);
        }

        [Test]
        public void CreatesLocomotiveRenovationProperly()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var renovationId = service
                .CreateLocomotiveRenovation(
                "userId",
                (RenovationVolume)0,
                "Skoda",
                1985,
                87215,
                (EngineType)0,
                1,
                "https://www.picture.com/picture.jpeg",
                "Great description");

            //Assert
            Assert.IsTrue(service.AllRenovations().Renovations.Any(r => r.Id == renovationId));
        }

        [Test]
        public void CreatesTrainCarRenovationProperly()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var renovationId = service
                .CreateTrainCarRenovation(
                "userId", 
                (RenovationVolume)0, 
                "Skoda", 
                1985, 
                87215, 
                1, 
                (LuxuryLevel)0, 
                1, 
                "https://www.picture.com/picture.jpeg", 
                "Great description");

            //Assert
            Assert.IsTrue(service.AllRenovations().Renovations.Any(r => r.Id == renovationId));
        }

        [Test]
        public void CancelCancelsProperRenovations()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var realResult = service.CancelRenovation("trainCarRenovation", "Sorry brother!");
            var approvedResult = service.CancelRenovation("locomotiveRenovation", "Sorry but that's it.");
            var paidResult = service.CancelRenovation("paidRenovation", "Well I don't know");
            var cancelledRenvation = service.AllRenovations().Renovations.FirstOrDefault(r => r.Id == "trainCarRenovation");

            //Assert
            Assert.IsTrue(realResult);
            Assert.IsFalse(approvedResult);
            Assert.IsFalse(paidResult);
            Assert.IsNotNull(cancelledRenvation);
            Assert.IsTrue(cancelledRenvation.IsCancelled);
        }

        [Test]
        public void DetailsGivesRightData()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var realResult = service.Details("locomotiveRenovation");
            var fakeResult = service.Details("notRealRenovation");

            //Assert
            Assert.NotNull(realResult);
            Assert.AreEqual("locomotiveRenovation", realResult.Id);
            Assert.IsNull(fakeResult);
        }

        [Test]
        public void UpdateWorksProperlyOnTheRightRenovations()
        {
            //Arrane
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var notApproved = service.Update("trainCarRenovation", 5, 2000000m, "Best comment ever");
            var notPaid = service.Update("locomotiveRenovation", 5, 2000000m, "Best comment ever");
            var paidResult = service.Update("paidRenovation", 5, 2000000m, "Best comment ever");
            var renovation = service.AllRenovations().Renovations.FirstOrDefault(r => r.Id == "trainCarRenovation");

            //Assert
            Assert.IsTrue(notApproved);
            Assert.IsTrue(notPaid);
            Assert.IsFalse(paidResult);
            Assert.NotNull(renovation);
            Assert.IsTrue(renovation.IsApproved);
        }

        [Test]
        public void UploadPictureUploadsOnlyOnPaidRenovations()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var realResult = service.UploadPicture("paidRenovation", "https://www.picture.com/picturefortrain.jpeg");
            var fakeResult = service.UploadPicture("nonExistent", "https://www.picture.com/picturefortrain.jpeg");
            var notPaidResult = service.UploadPicture("locomotiveRenovation", "https://www.picture.com/picturefortrain.jpeg");

            //Assert
            Assert.IsTrue(realResult);
            Assert.IsFalse(fakeResult);
            Assert.IsFalse(notPaidResult);
        }

        [Test]
        public void PayIsPayingOnlyTheNecessaryRenovations()
        { 
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var realPayment = service.Pay("locomotiveRenovation", "userId");
            var deletedPayment = service.Pay("deletedLocomotiveRenovation", "userId");
            var alreadyDonePayment = service.Pay("paidRenovation", "userId");
            var notApprovedPayment = service.Pay("trainCarRenovation", "userId");

            //Assert
            Assert.True(realPayment);
            Assert.False(deletedPayment);
            Assert.True(alreadyDonePayment);
            Assert.False(notApprovedPayment);
        }

        [Test]
        public void AllRenovationsArePresentForAdmin()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var result = service.AllRenovations((GlobalSorting)0, 1, 5);
            var smallerResult = service.AllRenovations((GlobalSorting)0, 1, 2);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(smallerResult);
            Assert.AreEqual(5, result.Renovations.Count());
            Assert.AreEqual(2, smallerResult.Renovations.Count());
        }

        [Test]
        public void AllApiRenovations()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var categories = service.ApiRenovations().Count();

            //Assert
            Assert.NotNull(categories);
            Assert.AreEqual(2, categories);
        }

        [Test]
        public void AllCategoriesArePresent()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var categories = service.AllCategories().Count();

            //Assert
            Assert.NotNull(categories);
            Assert.AreEqual(1, categories);
        }


        [Test]
        public void AllInterrailsArePresent()
        {
            //Arrange
            var service = serviceProvider.GetService<IRenovationService>();

            //Act
            var interrails = service.AllInterrails().Count();

            //Assert
            Assert.NotNull(interrails);
            Assert.AreEqual(1, interrails);
        }
        private void SeedData(TrainsDbContext context)
        {
            var interrail = new Interrail { Length = 1435 };
            var category = new Category { Name = "SomeCategory" };
            var user = new User { Id = "userId", FirstName = "user", LastName = "user", Company = "User user", Email = "user@user.com", PasswordHash = "hashedPassword", };
            var newUser = new User { Id = "newUserId", FirstName = "user", LastName = "user", Company = "User user", Email = "user@user.com", PasswordHash = "hashedPassword", };
            var locomotive = new Locomotive
            {
                Model = "Lyudmila Orwell",
                Year = 1984,
                Series = 92403,
                EngineType = (EngineType)0,
                Interrail = interrail,
                TopSpeed = 160,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Diesel locomotive",
                Price = 2500000m,
                IsDeleted = false,
                IsForRenovation = true
            };
            
            var deletedLocomotive = new Locomotive
            {
                Model = "Lyudmila Douglas",
                Year = 1994,
                Series = 97403,
                EngineType = (EngineType)0,
                Interrail = interrail,
                TopSpeed = 160,
                Picture = "https://cdn.pixabay.com/photo/2017/09/07/22/14/locomotive-2726914_960_720.jpg",
                Description = "Diesel locomotive",
                Price = 2500000m,
                IsDeleted = true,
                IsForRenovation = true
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
                IsForRenovation = true
            };


            context.Renovations.AddRange(new Renovation[]
                {
                    new Renovation
                    {
                         Id = "locomotiveRenovation",
                         RenovationVolume = (RenovationVolume)0,
                        RenovationType = (RenovationType)0,
                        DateCreated = DateTime.UtcNow,
                        User = user,
                        Locomotive = locomotive,
                        IsApproved = true
                    },
                    new Renovation
                    {
                         Id = "paidRenovation",
                         RenovationVolume = (RenovationVolume)0,
                        RenovationType = (RenovationType)0,
                        DateCreated = DateTime.UtcNow,
                        User = user,
                        Locomotive = locomotive,
                        IsApproved = true,
                        IsPaid = true
                    },
                    new Renovation
                    {
                         Id = "newRenovationPaid",
                         RenovationVolume = (RenovationVolume)0,
                        RenovationType = (RenovationType)0,
                        DateCreated = DateTime.UtcNow,
                        User = newUser,
                        Locomotive = locomotive,
                        IsApproved = true,
                        IsPaid = true
                    },
                    new Renovation
                    {
                         Id = "deletedLocomotiveRenovation",
                         RenovationVolume = (RenovationVolume)0,
                        RenovationType = (RenovationType)0,
                        DateCreated = DateTime.UtcNow,
                        User = user,
                        Locomotive = deletedLocomotive
                    },
                    new Renovation
                    {
                        Id = "trainCarRenovation",
                        RenovationVolume = (RenovationVolume)1,
                        RenovationType = (RenovationType)1,
                        DateCreated = DateTime.UtcNow,
                        User = user,
                        TrainCar = trainCar
                    }
            });

            context.SaveChanges();
        }
    }
}
