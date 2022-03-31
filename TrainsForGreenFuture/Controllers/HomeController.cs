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

        public HomeController(ILogger<HomeController> logger, 
            TrainsDbContext dbContext,
            ILocomotiveService locomotiveService)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.locomotiveService = locomotiveService;
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
                    AddUrlRef = "/Locomotives/Add"},
                new TrainsGenericViewModel
                {
                    TypeName = "Train Cars",
                ImageUrl = "https://bit.ly/3NxLaOe",
                    Count = dbContext.TrainCars.Count(), 
                    UrlRef = "/TrainCars/All",
                    AddUrlRef = "/TrainCars/Add"
                },
                new TrainsGenericViewModel
                {
                    TypeName = "Trains",
                    ImageUrl = "https://bit.ly/3JWAre5",
                    Count = dbContext.Trains.Count(), 
                    UrlRef = "/Trains/All",
                    AddUrlRef = "/Trains/Add"
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