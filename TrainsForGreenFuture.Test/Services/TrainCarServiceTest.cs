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
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class TrainCarServiceTest
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
                .AddSingleton<TrainDbContext, TrainCarService>(ls =>
                new TrainCarService(
                    ls.GetRequiredService<TrainsDbContext>(),
                    mapper))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<TrainsDbContext>();
            SeedData(context);
        }

        [Test]
        public void AllTrainCarsExceptDeletedAndForRenovation()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var trainCars = service.AllTrainCars().Count();

            //Assert
            Assert.NotNull(trainCars);
            Assert.AreEqual(2, trainCars);
        }

        [Test]
        public void CreateSuccessfullyTrainCars()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var successfulAddId = service.Create(
                "Sleeper Douglas Adams",
                2000,
                55042,
                1,
                33,
                (LuxuryLevel)0,
                2,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var trainCarsCount = service.AllTrainCars().Count();
            var containsTrainCar = service.AllTrainCars().Any(l => l.Id == successfulAddId);

            //Assert
            Assert.AreEqual(3, trainCarsCount);
            Assert.True(containsTrainCar);
        }

        [Test]
        public void EditsOnlyExistentTrainCars()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var successfulEdit = service.Edit(
                1,
                "Skoda Douglas Adams",
                2000,
                55042,
                1,
                33,
                (LuxuryLevel)0,
                2,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var failedEdit = service.Edit(
                87,
                "Skoda Orwell",
                2000,
                55042,
                1,
                33,
                (LuxuryLevel)0,
                2,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var deletedEdit = service.Edit(
                3,
                "Skoda Douglas Adams",
                2000,
                55042,
                1,
                33,
                (LuxuryLevel)0,
                2,
                "https://www.nopicture.com/image.jpg",
                "No description",
                2000000m);

            var forRenovationEdit = service.Edit(
                4,
                "Skoda Douglas Adams",
                2000,
                55042,
                1,
                33,
                (LuxuryLevel)0,
                2,
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
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var trainCar = service.Details(1);
            var deletedTrainCar = service.Details(3);
            var forRenovationTrainCar = service.Details(4);
            var nonExistentTrainCar = service.Details(35);

            //Assert
            Assert.NotNull(trainCar);
            Assert.Null(deletedTrainCar);
            Assert.Null(forRenovationTrainCar);
            Assert.Null(nonExistentTrainCar);
            Assert.That(trainCar.Model == "Sleeper Orwell");
        }

        [Test]
        public void FormDetailsGivesAProperTrainCar()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var trainCar = service.FormDetails(2);
            var deletedTrainCar = service.FormDetails(3);
            var forRenovationTrainCar = service.FormDetails(4);
            var nonExistentTrainCar = service.FormDetails(55);

            //Assert
            Assert.NotNull(trainCar);
            Assert.Null(deletedTrainCar);
            Assert.Null(forRenovationTrainCar);
            Assert.Null(nonExistentTrainCar);
            Assert.That(trainCar.Model == "Restaurant Orwell");
        }

        [Test]
        public void RemoveIsRemovingValidTrainCars()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var realTrainCarResult = service.Remove(1);
            var trainCarsLeft = service.AllTrainCars().Count();
            var nonExistentTrainCarResult = service.Remove(15);

            //Assert
            Assert.True(realTrainCarResult);
            Assert.AreEqual(1, trainCarsLeft);
            Assert.False(nonExistentTrainCarResult);
        }

        [Test]
        public void AllInterrailsArePresent()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var interrails = service.AllInterrails().Count();

            //Assert
            Assert.NotNull(interrails);
            Assert.AreEqual(3, interrails);
        }

        [Test]
        public void AllCategoriesArePresent()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var categories = service.AllCategories().Count();

            //Assert
            Assert.NotNull(categories);
            Assert.AreEqual(4, categories);
        }

        [Test]
        public void GetCategoryNameGivesProperName()
        {
            //Arrange
            var service = serviceProvider.GetService<TrainDbContext>();

            //Act
            var category = service.GetCategoryName(3);
            var nonEistentCategory = service.GetCategoryName(204);

            //Assert
            Assert.NotNull(category);
            Assert.Null(nonEistentCategory);
            Assert.AreEqual(category, "Sleeper");
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
