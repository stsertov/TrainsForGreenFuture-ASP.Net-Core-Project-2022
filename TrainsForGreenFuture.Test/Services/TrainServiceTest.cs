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
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class TrainServiceTest
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
                .AddSingleton<ITrainService, TrainService>(ts =>
                new TrainService(
                    ts.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        [Test]
        public void AllTrainsExceptDeletedAndForRenovation()
        {
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();

            //Act
            var trains = service.AllTrains().Count();

            //Assert
            Assert.NotNull(trains);
            Assert.AreEqual(2, trains);
        }

        [Test]
        public void AllInterrailsArePresent()
        {
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();

            //Act
            var interrails = service.AllInterrails().Count();

            //Assert
            Assert.NotNull(interrails);
            Assert.AreEqual(3, interrails);
        }

        [Test]
        public void CreateSuccessfullyTrains()
        {
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();

            //Act
            var successfulAddId = service.Create(
                "Skoda Douglas Adams",
                2000,
                82342,
                (LuxuryLevel)1,
                8,
                (EngineType)0,
                2,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var trainsCount = service.AllTrains().Count();
            var containsTrain = service.AllTrains().Any(l => l.Id == successfulAddId);

            //Assert
            Assert.AreEqual(3, trainsCount);
            Assert.True(containsTrain);
        }

        [Test]
        public void EditsOnlyExistentTrains()
        {
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();

            //Act
            var successfulEdit = service.Edit(
                1,
                "Skoda Douglas Adams",
                2000, 
                82342,
                (LuxuryLevel)1,
                8,
                (EngineType)0,
                2,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var failedEdit = service.Edit(
                87,
                "Skoda Douglas Adams",
                2000,
                82342,
                (LuxuryLevel)1,
                8,
                (EngineType)0,
                2,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var deletedEdit = service.Edit(
                3,
                "Skoda Douglas Adams",
                2000,
                82342,
                (LuxuryLevel)1,
                8,
                (EngineType)0,
                2,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var forRenovationEdit = service.Edit(
                4,
                "Skoda Douglas Adams",
                2000,
                82342,
                (LuxuryLevel)1,
                8,
                (EngineType)0,
                2,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            //Assert
            Assert.True(successfulEdit);
            Assert.False(failedEdit);
            Assert.False(deletedEdit);
            Assert.False(forRenovationEdit);
        }

        [Test]
        public void DetailsGivesAProperTrain()
        {
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();

            //Act
            var Train = service.Details(1);
            var deletedTrain = service.Details(3);
            var forRenovationTrain = service.Details(4);
            var nonExistentTrain = service.Details(35);

            //Assert
            Assert.NotNull(Train);
            Assert.Null(deletedTrain);
            Assert.Null(forRenovationTrain);
            Assert.Null(nonExistentTrain);
            Assert.That(Train.Model == "Train Orwell");
        }

        [Test]
        public void FormDetailsGivesAProperTrain()
        {
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();

            //Act
            var train = service.FormDetails(2);
            var deletedTrain = service.FormDetails(3);
            var forRenovationTrain = service.FormDetails(4);
            var nonExistentTrain = service.FormDetails(55);

            //Assert
            Assert.NotNull(train);
            Assert.Null(deletedTrain);
            Assert.Null(forRenovationTrain);
            Assert.Null(nonExistentTrain);
            Assert.That(train.Model == "Train Douglas");
        }

        [Test]
        public void RemoveIsRemovingValidTrains()
        {
            //Arrange
            var service = serviceProvider.GetService<ITrainService>();

            //Act
            var realTrainResult = service.Remove(1);
            var trainsLeft = service.AllTrains().Count();
            var nonExistentTrainResult = service.Remove(15);

            //Assert
            Assert.True(realTrainResult);
            Assert.AreEqual(1, trainsLeft);
            Assert.False(nonExistentTrainResult);
        }

        [TearDown]
        public void TearDown()
           => dbContext.Dispose();

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
