namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    public class TrainsController : Controller
    {
        private ITrainService service;
        public TrainsController(ITrainService service)
        {
            this.service = service;
        }
        public IActionResult All()
           => View(service.AllTrains());

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
