namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    using static Areas.RolesConstants;
    public class TrainCarsController : Controller
    {
        private ITrainCarService service;
        private IMapper mapper;
        public TrainCarsController(ITrainCarService service,
            IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }
        public IActionResult All()
              => View(service.AllTrainCars());




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
