namespace TrainsForGreenFuture.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Renovations;
    using TrainsForGreenFuture.Extensions;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    using static TrainsForGreenFuture.Areas.Admin.AdminConstants;
    public class RenovationsController : Controller
    {
        private IRenovationService service;

        public RenovationsController(IRenovationService service)
        {
            this.service = service;
        }

        public IActionResult All([FromQuery] AllRenovationsViewModel renovations)
        {
            var renovationModel = new AllRenovationsViewModel();

            if (User.Identity.IsAuthenticated)
            {
                renovationModel = service.All(
                    User.Id(),
                    renovations.Sorting,
                    renovations.CurrentPage,
                    AllRenovationsViewModel.RenovationsPerPage);
            }
            else
            {
                renovationModel = service.AllFinished(
                    renovations.Sorting,
                    renovations.CurrentPage,
                    AllRenovationsViewModel.RenovationsPerPage);
            }

            return View(renovationModel);
        }


        [Authorize]
        public IActionResult ApplyForLocomotive()
            => View(new RenovationLocomotiveApplyFormModel { Interrails = service.AllInterrails() });

        [HttpPost]
        [Authorize]
        public IActionResult ApplyForLocomotive(RenovationLocomotiveApplyFormModel locomotive)
        {
            if (!Enum.TryParse<RenovationVolume>(locomotive.RenovationVolume, out var parsedRenovationVolume))
            {
                ModelState.AddModelError("Invalid Renovation Volume", "We do not offer this type of renovation.");
            }

            if (!Enum.TryParse(locomotive.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(locomotive.EngineType, "We do not offer this engine type.");
            }

            if (!service.AllInterrails().Any(i => i.Id == locomotive.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!ModelState.IsValid)
            {
                locomotive.Interrails = service.AllInterrails();
                return View(locomotive);
            }


            service.CreateLocomotiveRenovation(
                User.Id(),
                parsedRenovationVolume,
                locomotive.Model,
                locomotive.Year.Value,
                locomotive.Series.Value,
                parsedEngineType,
                locomotive.InterrailId.Value,
                locomotive.Picture,
                locomotive.Description);


            return Redirect("/Renovations/All");
        }

        [Authorize]
        public IActionResult ApplyForTrainCar()
            => View(new RenovationTrainCarApplyFormModel
            {
                Interrails = service.AllInterrails(),
                Categories = service.AllCategories()
            });

        [HttpPost]
        [Authorize]
        public IActionResult ApplyForTrainCar(RenovationTrainCarApplyFormModel trainCar)
        {
            if (!Enum.TryParse<RenovationVolume>(trainCar.RenovationVolume, out var parsedRenovationVolume))
            {
                ModelState.AddModelError("Invalid Renovation Volume", "We do not offer this type of renovation.");
            }

            if (!Enum.TryParse(trainCar.LuxuryLevel, out LuxuryLevel parsedLuxuryLevel))
            {
                ModelState.AddModelError(trainCar.LuxuryLevel, "We do not offer this type of luxury.");
            }

            if (!service.AllInterrails().Any(i => i.Id == trainCar.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!ModelState.IsValid)
            {
                trainCar.Interrails = service.AllInterrails();
                trainCar.Categories = service.AllCategories();
                return View(trainCar);
            }


            service.CreateTrainCarRenovation(
                User.Id(),
                parsedRenovationVolume,
                trainCar.Model,
                trainCar.Year.Value,
                trainCar.Series.Value,
                trainCar.CategoryId.Value,
                parsedLuxuryLevel,
                trainCar.InterrailId.Value,
                trainCar.Picture,
                trainCar.Description);


            return Redirect("/Renovations/All");
        }
       
        public IActionResult Details(string id)
        {
            var renovation = service.Details(id);

            if(renovation == null)
            {
                return Redirect("/Home/Index");
            }

            return View(renovation);
        }
    }
}
