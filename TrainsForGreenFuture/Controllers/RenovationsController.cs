namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Renovations;

    public class RenovationsController : Controller
    {
        private IRenovationService service;

        public RenovationsController(IRenovationService service)
        {
            this.service = service;
        }

        public IActionResult All(AllRenovationsViewModel renovations)
        {
            var renovationModel = service.All(
             renovations.Sorting,
             renovations.CurrentPage,
             AllRenovationsViewModel.RenovationsPerPage);

            return View(renovationModel);
        }
    }
}
