namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Trains;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class TrainsController : AdminController
    {
        private const string TrainAllRoute = "/Trains/All";
        private ITrainService service;

        public TrainsController(ITrainService service)
            => this.service = service;
        

        public IActionResult Add()
            => View(new TrainFormModel
            {
                Interrails = service.AllInterrails()
            });

        [HttpPost]
        public IActionResult Add(TrainFormModel train)
        {

            if (!Enum.TryParse(train.LuxuryLevel, out LuxuryLevel parsedLuxuryType))
            {
                ModelState.AddModelError("Invalid luxury level.", "We do not offer this luxury level.");
            }
            
            if (!Enum.TryParse(train.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(train.EngineType, "We do not offer this type of engine.");
            }

            if (!service.AllInterrails().Any(i => i.Id == train.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }          

            if (!ModelState.IsValid)
            {
                train.Interrails = service.AllInterrails();
                return View(train);
            }

            var trainCarId = service.Create(
                train.Model,
                train.Year.Value,
                train.Series.Value,
                parsedLuxuryType,
                train.TrainCarCount.Value,
                parsedEngineType,
                train.InterrailId.Value,
                train.TopSpeed.Value,
                train.Picture,
                train.Description,
                train.Price.Value);

            return Redirect(TrainAllRoute);
        }

        public IActionResult Edit(int id)
        {
            var train = service.FormDetails(id);

            if (train == null)
            {
                return Redirect(TrainAllRoute);
            }

            train.Interrails = service.AllInterrails();

            return View(train);
        }

        [HttpPost]
        public IActionResult Edit(int id, TrainFormModel train)
        {
            if (!Enum.TryParse(train.LuxuryLevel, out LuxuryLevel parsedLuxuryType))
            {
                ModelState.AddModelError("Invalid luxury level.", "We do not offer this luxury level.");
            }

            if (!Enum.TryParse(train.EngineType, out EngineType parsedEngineType))
            {
                ModelState.AddModelError(train.EngineType, "We do not offer this type of engine.");
            }

            if (!service.AllInterrails().Any(i => i.Id == train.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!ModelState.IsValid)
            {
                train.Interrails = service.AllInterrails();
                return View(train);
            }

            var isEdited = service.Edit(
                id,
                train.Model,
                train.Year.Value,
                train.Series.Value,
                parsedLuxuryType,
                train.TrainCarCount.Value,
                parsedEngineType,
                train.InterrailId.Value,
                train.TopSpeed.Value,
                train.Picture,
                train.Description,
                train.Price.Value
                );

            if (!isEdited)
            {
                train.Interrails = service.AllInterrails();
                return View(train);
            }

            return Redirect(TrainAllRoute);
        }

        public IActionResult Delete(int id)
        {
            var isRemoved = service.Remove(id);

            if (isRemoved)
            {
                return Redirect("/Home/Trains");
            }

            return Redirect(TrainAllRoute);
        }
    }
}
