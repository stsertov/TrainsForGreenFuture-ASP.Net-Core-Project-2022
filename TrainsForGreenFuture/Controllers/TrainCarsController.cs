namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    public class TrainCarsController : Controller
    {
        private ITrainCarService service;
        public TrainCarsController(ITrainCarService service)
        {
            this.service = service;
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
