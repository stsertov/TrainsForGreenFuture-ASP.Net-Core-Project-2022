﻿namespace TrainsForGreenFuture.Controllers
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
                    ImageUrl = "https://bit.ly/3iyFDsz", 
                    Count = dbContext.Locomotives.Count(), 
                    UrlRef = "/Locomotives/All",
                    AddUrlRef = "/Locomotives/Add"},
                new TrainsGenericViewModel
                {
                    TypeName = "Train Cars",
                ImageUrl = "https://bit.ly/3tEPiEk",
                    Count = dbContext.TrainCars.Count(), 
                    UrlRef = "/TrainCars/All",
                    AddUrlRef = "/TrainCars/Add"
                },
                new TrainsGenericViewModel
                {
                    TypeName = "Trains",
                    ImageUrl = "https://bit.ly/35bwR0z",
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