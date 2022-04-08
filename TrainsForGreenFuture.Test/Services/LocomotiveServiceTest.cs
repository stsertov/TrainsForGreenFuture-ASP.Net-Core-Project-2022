namespace TrainForGreenFuture.Test.Services
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Infrastructure;
    using TrainsForGreenFuture.Core.Services;
    using TrainsForGreenFuture.Infrastructure.Data;
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
                .AddSingleton<ILocomotiveService, LocomotiveService>(ls =>
                new LocomotiveService(
                    mapper,
                    ls.GetRequiredService<TrainsDbContext>()))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }


        [Test]
        public void AllLocomotiveExceptDeletedAndForRenovation()
        {
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();

            //Act
            var locomotives = service.AllLocomotives().Count();

            //Assert
            Assert.NotNull(locomotives);
            Assert.AreEqual(2, locomotives);
        }

        [Test]
        public void AllInterrailsArePresent()
        {
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();

            //Act
            var interrails = service.AllInterrails().Count();

            //Assert
            Assert.NotNull(interrails);
            Assert.AreEqual(3, interrails);
        }

        [Test]
        public void CreateSuccessfullyLocomotives()
        {
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();

            //Act
            var successfulAddId = service.Create(
                "Skoda Douglas Adams",
                2000, 82342,
                (EngineType)0,
                1,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var locomotivesCount = service.AllLocomotives().Count();
            var containsLocomotive = service.AllLocomotives().Any(l => l.Id == successfulAddId);

            //Assert
            Assert.AreEqual(3, locomotivesCount);
            Assert.True(containsLocomotive);
        }

        [Test]
        public void EditsOnlyExistentLocomotives()
        {
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();

            //Act
            var successfulEdit = service.Edit(
                1,
                "Skoda Douglas Adams",
                2000, 82342,
                (EngineType)0,
                1,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var failedEdit = service.Edit(
                87,
                "Skoda Douglas Adams",
                2000, 82342,
                (EngineType)0,
                1,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m); 

            var deletedEdit = service.Edit(
                3,
                "Skoda Douglas Adams",
                2000, 82342,
                (EngineType)0,
                1,
                200,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var forRenovationEdit = service.Edit(
                4,
                "Skoda Douglas Adams",
                2000, 82342,
                (EngineType)0,
                1,
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
        public void DetailsGivesAProperLocomotive()
        {
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();

            //Act
            var locomotive = service.Details(1);
            var deletedLocomotive = service.Details(3);
            var forRenovationLocomotive = service.Details(4);
            var nonExistentLocomotive = service.Details(35);

            //Assert
            Assert.NotNull(locomotive);
            Assert.Null(nonExistentLocomotive);
            Assert.Null(deletedLocomotive);
            Assert.Null(forRenovationLocomotive);
            Assert.That(locomotive.Model == "Skoda Orwell");
        }

        [Test]
        public void FormDetailsGivesAProperLocomotive()
        {
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();

            //Act
            var locomotive = service.FormDetails(2);
            var deletedLocomotive = service.FormDetails(3);
            var forRenovationLocomotive = service.FormDetails(4);
            var nonExistentLocomotive = service.FormDetails(55);

            //Assert
            Assert.NotNull(locomotive);
            Assert.Null(deletedLocomotive);
            Assert.Null(forRenovationLocomotive);
            Assert.Null(nonExistentLocomotive);
            Assert.That(locomotive.Model == "Lyudmila Orwell");
        }

        [Test]
        public void RemoveIsRemovingValidLocomotives()
        {
            //Arrange
            var service = serviceProvider.GetService<ILocomotiveService>();

            //Act
            var realLocomotiveResult = service.Remove(1);
            var locomotivesLeft = service.AllLocomotives().Count();
            var nonExistentLocomotiveResult = service.Remove(15);

            //Assert
            Assert.True(realLocomotiveResult);
            Assert.AreEqual(1, locomotivesLeft);
            Assert.False(nonExistentLocomotiveResult);
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