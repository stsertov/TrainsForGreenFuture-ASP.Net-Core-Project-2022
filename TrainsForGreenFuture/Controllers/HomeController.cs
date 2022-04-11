namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Models;
    using TrainsForGreenFuture.Models.Home;

    public class HomeController : Controller
    {
        private ILocomotiveService locomotiveService;
        private ITrainCarService trainCarService;
        private ITrainService trainService;

        public HomeController(ILocomotiveService locomotiveService,
            ITrainCarService trainCarService,
            ITrainService trainService)
        {
            this.locomotiveService = locomotiveService;
            this.trainCarService = trainCarService;
            this.trainService = trainService;
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
                    ImageUrl = "https://bit.ly/3LWqqxW",
                    Count = locomotiveService.AllLocomotives().Count(),
                    UrlRef = "/Locomotives/All",
                    AddUrlRef = "/Admin/Locomotives/Add"},
                new TrainsGenericViewModel
                {
                    TypeName = "Train Cars",
                ImageUrl = "https://bit.ly/3JHx8ql",
                    Count = trainCarService.AllTrainCars().Count(),
                    UrlRef = "/TrainCars/All",
                    AddUrlRef = "/Admin/TrainCars/Add"
                },
                new TrainsGenericViewModel
                {
                    TypeName = "Trains",
                    ImageUrl = "https://bit.ly/3jrYXYJ",
                    Count = trainService.AllTrains().Count(), 
                    UrlRef = "/Trains/All",
                    AddUrlRef = "/Admin/Trains/Add"
                }
            };

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