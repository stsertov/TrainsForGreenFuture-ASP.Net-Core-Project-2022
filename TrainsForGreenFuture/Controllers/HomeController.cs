namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Models;
    using TrainsForGreenFuture.Models.Home;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private TrainsDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, TrainsDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
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
                    ImageUrl = "https://localhost:7260/images/locomotives.png", 
                    Count = dbContext.Locomotives.Count(), 
                    UrlRef = "/Locomotives/All"},
                new TrainsGenericViewModel
                {
                    TypeName = "Train Cars",
                ImageUrl = "https://localhost:7260/images/traincars.png",
                    Count = dbContext.TrainCars.Count(), 
                    UrlRef = "/TrainCars/All"
                },
                new TrainsGenericViewModel
                {
                    TypeName = "Trains",
                    ImageUrl = "https://localhost:7260/images/trains.png",
                    Count = dbContext.Trains.Count(), 
                    UrlRef = "/Trains/All"}
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