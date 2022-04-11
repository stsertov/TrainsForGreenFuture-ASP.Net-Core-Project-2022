namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Areas.Admin.Models.Renovations;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Renovations;

    public class RenovationsController : AdminController
    {
        private const string RenovationsAllRoute = "/Admin/Renovations/All";
        private IRenovationService service;

        public RenovationsController(IRenovationService service)
            => this.service = service;

        public IActionResult All([FromQuery] AllRenovationsViewModel renovations)
            => View(service.AllRenovations(
                renovations.Sorting, 
                renovations.CurrentPage, 
                AllRenovationsViewModel.RenovationsPerPage));

        public IActionResult Update(string id)
        {
            var renovation = service.Details(id);

            if(renovation == null)
            {
                return Redirect(RenovationsAllRoute);
            }

            return View(renovation);
        }
        
        [HttpPost]
        public IActionResult Update(string id, RenovationAdminFromModel renovation)
        {
            if(!ModelState.IsValid)
            {
                var detailRenovation = service.Details(id);
                return View(detailRenovation);
            }

            var result = service.Update(
                id,
                renovation.Deadline.Value,
                renovation.Price.Value,
                renovation.Comment);

            if(!result)
            {
                return Redirect("/Home/Error");
            }

            return Redirect(RenovationsAllRoute);
        }

        public IActionResult UploadPicture(string id)
            => View(new RenovationAdminPictureFormModel { Id = id });

        [HttpPost]
        public IActionResult UploadPicture(string id, RenovationAdminPictureFormModel uploadPic)
        {

            if(!Uri.IsWellFormedUriString(uploadPic.RenovationPicture, UriKind.Absolute))
            {
                return Redirect("/Home/Error");
            }

            var result = service.UploadPicture(id, uploadPic.RenovationPicture);

            if(!result)
            {
                return Redirect("/Home/Error");
            }

            return Redirect($"{RenovationsAllRoute}/{id}");
        }


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

            return Redirect(RenovationsAllRoute);
        }
    }
}
