namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Renovations;

    public class RenovationsController : AdminController
    {
        private IRenovationService service;

        public RenovationsController(IRenovationService service)
        {
            this.service = service;
        }

        public IActionResult All([FromQuery] AllRenovationsViewModel renovations)
            => View(service.AllRenovations(
                renovations.Sorting, 
                renovations.CurrentPage, 
                AllRenovationsViewModel.RenovationsPerPage));

        public IActionResult Review()
            => View(); 
        
        [HttpPost]
        public IActionResult Review(string id)
            => View();


        public IActionResult Cancel(string id)
        {
            var renovation = service.Details(id);

            return View(renovation);
        }
        
        [HttpPost]
        public IActionResult Cancel(string id, string comment)
        {
            var result = service.CancelRenovation(id, comment);

            if(!result)
            {
                return BadRequest();
            }

            return Redirect("/Admin/Renovations/All");
        }
    }
}
