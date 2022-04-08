namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.TrainCars;

    public class TrainCarsController : Controller
    {
        private const string TrainCarCacheKey = "traincarsall";
        private TrainDbContext service;
        private IMemoryCache cache;
        public TrainCarsController(TrainDbContext service,
            IMemoryCache cache)
        {
            this.service = service;
            this.cache = cache;
        }
        public IActionResult All()
        {
            var trainCars = cache.Get<IEnumerable<TrainCarViewModel>>(TrainCarCacheKey);

            if (trainCars == null)
            {
                trainCars = service.AllTrainCars();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(5));

                cache.Set(TrainCarCacheKey, trainCars, cacheOptions);
            }

            return View(trainCars);
        }

        public IActionResult Details(int id)
        {
            var trainCar = service.Details(id);

            if (trainCar == null)
            {
                return Redirect("/TrainCars/All");
            }

            return View(trainCar);
        }
    }
}
