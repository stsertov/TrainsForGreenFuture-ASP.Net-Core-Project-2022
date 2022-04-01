namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Models;
    using TrainsForGreenFuture.Models.Home;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private TrainsDbContext dbContext;
        private ILocomotiveService locomotiveService;
        private ITrainCarService trainCarService;

        public HomeController(ILogger<HomeController> logger, 
            TrainsDbContext dbContext,
            ILocomotiveService locomotiveService,
            ITrainCarService trainCarService)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.locomotiveService = locomotiveService;
            this.trainCarService = trainCarService;
        }

        public IActionResult Index()
            => View();

        public IActionResult Trains()
        {
            var genericTrains = new List<TrainsGenericViewModel>()
            {
                new TrainsGenericViewModel
                {
                    TypeName = "Locomotives", 
                    ImageUrl = "https://bit.ly/3qQcMod", 
                    Count = locomotiveService.AllLocomotives().Count(), 
                    UrlRef = "/Locomotives/All",
                    AddUrlRef = "/Admin/Locomotives/Add"},
                new TrainsGenericViewModel
                {
                    TypeName = "Train Cars",
                ImageUrl = "https://bit.ly/3NxLaOe",
                    Count = trainCarService.AllTrainCars().Count(),
                    UrlRef = "/TrainCars/All",
                    AddUrlRef = "/Admin/TrainCars/Add"
                },
                new TrainsGenericViewModel
                {
                    TypeName = "Trains",
                    ImageUrl = "https://bit.ly/3JWAre5",
                    Count = dbContext.Trains.Count(), 
                    UrlRef = "/Trains/All",
                    AddUrlRef = "/Admin/Trains/Add"
                }
            };

            return View(genericTrains);
        }

        public IActionResult Renovation()
            => View();
        public IActionResult Privacy() 
            => View();

        public IActionResult About()
            => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}