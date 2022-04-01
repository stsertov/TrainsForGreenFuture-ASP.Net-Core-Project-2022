namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    public class LocomotivesController : Controller
    {
        private ILocomotiveService service;
        public LocomotivesController(IMapper mapper,
            ILocomotiveService service)
        {
            this.service = service;
        }
        public IActionResult All()
            => View(service.AllLocomotives());

        public IActionResult Details(int id)
        {
            var locomotive = service.Details(id);

            if (locomotive == null)
            {
                return Redirect("/Locomotives/All");
            }

            return View(locomotive);
        }
    }
}
