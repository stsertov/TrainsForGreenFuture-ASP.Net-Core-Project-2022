namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class LocomotivesController : AdminController
    {
        private const string AllLocomotivesRoute = "/Locomotives/All";
        private ILocomotiveService service;

        public LocomotivesController(ILocomotiveService service)
            => this.service = service;
        

        public IActionResult Add()
        {
            return View(new LocomotiveFormModel { Interrails = service.AllInterrails() });
        }

        [HttpPost]
        public IActionResult Add(LocomotiveFormModel locomotive)
        {
            if (!Enum.TryParse(locomotive.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(locomotive.EngineType, "We do not offer this engine type.");
            }

            if (!service.AllInterrails().Any(i => i.Id == locomotive.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!Uri.IsWellFormedUriString(locomotive.Picture, UriKind.Absolute))
            {
                ModelState.AddModelError("Invalid Url", "Url is invalid.");
            }

            if (!ModelState.IsValid)
            {
                locomotive.Interrails = service.AllInterrails();
                return View(locomotive);
            }

            var locomotiveId = service.Create(
                locomotive.Model,
                locomotive.Year.Value,
                locomotive.Series.Value,
                parsedEngineType,
                locomotive.InterrailId.Value,
                locomotive.TopSpeed.Value,
                locomotive.Picture,
                locomotive.Description,
                locomotive.Price.Value);

            return Redirect(AllLocomotivesRoute);
        }

        public IActionResult Edit(int id)
        {

            var locomotive = service.FormDetails(id);

            if (locomotive == null)
            {
                return Redirect(AllLocomotivesRoute);
            }

            locomotive.Interrails = service.AllInterrails();

            return View(locomotive);
        }

        [HttpPost]
        public IActionResult Edit(int id, LocomotiveFormModel locomotive)
        {
            if (!Enum.TryParse(locomotive.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(locomotive.EngineType, "We do not offer this engine type.");
            }

            if (!Uri.IsWellFormedUriString(locomotive.Picture, UriKind.Absolute))
            {
                ModelState.AddModelError("Invalid Url", "Url is invalid.");
            }

            if (!ModelState.IsValid)
            {
                locomotive.Interrails = service.AllInterrails();
                return View(locomotive);
            }

            var isEdited = service.Edit(
                id,
                locomotive.Model,
                locomotive.Year.Value,
                locomotive.Series.Value,
                parsedEngineType,
                locomotive.InterrailId.Value,
                locomotive.TopSpeed.Value,
                locomotive.Picture,
                locomotive.Description,
                locomotive.Price.Value
                );

            if (!isEdited)
            {
                locomotive.Interrails = service.AllInterrails();
                return View(locomotive);
            }

            return Redirect(AllLocomotivesRoute);
        }

        public IActionResult Delete(int id)
        {

            var isRemoved = service.Remove(id);

            if (isRemoved)
            {
                return Redirect("/Home/Trains");
            }

            return Redirect(AllLocomotivesRoute);
        }
    }
}
