namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System.Diagnostics;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Models;
    using TrainsForGreenFuture.Models.Home;

    public class HomeController : Controller
    {
        private const string GenericTrainsCacheKey = "generictrains";
        private ILocomotiveService locomotiveService;
        private ITrainCarService trainCarService;
        private ITrainService trainService;
        private IMemoryCache cache;
        public HomeController(ILocomotiveService locomotiveService,
            ITrainCarService trainCarService,
            ITrainService trainService,
            IMemoryCache cache)
        {
            this.locomotiveService = locomotiveService;
            this.trainCarService = trainCarService;
            this.trainService = trainService;
            this.cache = cache;
        }

        public IActionResult Index()
            => View();

        public IActionResult Trains()
        {
            var genericTrains = cache.Get<IEnumerable<TrainsGenericViewModel>>(GenericTrainsCacheKey);

            if (genericTrains == null)
            {
                genericTrains = new List<TrainsGenericViewModel>()
                {
                    new TrainsGenericViewModel
                    {
                        TypeName = "Locomotives",
                        ImageUrl = "https://bit.ly/3OdHMsj",
                        Count = locomotiveService.AllLocomotives().Count(),
                        UrlRef = "/Locomotives/All",
                        AddUrlRef = "/Admin/Locomotives/Add"},
                    new TrainsGenericViewModel
                    {
                        TypeName = "Train Cars",
                    ImageUrl = "https://bit.ly/3Ell2BZ",
                        Count = trainCarService.AllTrainCars().Count(),
                        UrlRef = "/TrainCars/All",
                        AddUrlRef = "/Admin/TrainCars/Add"
                    },
                    new TrainsGenericViewModel
                    {
                        TypeName = "Trains",
                        ImageUrl = "https://bit.ly/3M65vJ2",
                        Count = trainService.AllTrains().Count(),
                        UrlRef = "/Trains/All",
                        AddUrlRef = "/Admin/Trains/Add"
                    }
                };

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromDays(5));

                cache.Set(GenericTrainsCacheKey, genericTrains, cacheOptions);
            }


            return View(genericTrains);
        }

        public IActionResult Privacy()
            => View();

        public IActionResult About()
            => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }
}