namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Core.Models.Trains;

    public class TrainsController : Controller
    {
        private const string TrainCacheKey = "trainsall";
        private ITrainService service;
        private IMemoryCache cache;
        public TrainsController(ITrainService service,
            IMemoryCache cache)
        {
            this.service = service;
            this.cache = cache;
        }
        public IActionResult All()
        {
            var trains = cache.Get<IEnumerable<TrainViewModel>>(TrainCacheKey);

            if (trains == null)
            {
                trains = service.AllTrains();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(5));

                cache.Set(TrainCacheKey, trains, cacheOptions);
            }

            return View(trains);
        }

        public IActionResult Details(int id)
        {
            var train = service.Details(id);

            if (train == null)
            {
                return Redirect("/Trains/All");
            }

            return View(train);
        }

    }
}
