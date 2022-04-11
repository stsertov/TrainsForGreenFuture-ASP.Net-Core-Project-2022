namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class TrainCarsController : AdminController
    {
        private const string TrainCarAllRoute = "/TrainCars/All";
        private ITrainCarService service;

        public TrainCarsController(ITrainCarService service)
            => this.service = service;


        public IActionResult Add()
            => View(new TrainCarFormModel
            {
                Interrails = service.AllInterrails(),
                Categories = service.AllCategories()
            });

        [HttpPost]
        public IActionResult Add(TrainCarFormModel trainCar)
        {

            if (!Enum.TryParse(trainCar.LuxuryLevel, out LuxuryLevel parsedLuxuryType))
            {
                ModelState.AddModelError("Invalid luxury level.", "We do not offer this luxury level.");
            }

            if (!service.AllInterrails().Any(i => i.Id == trainCar.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!service.AllCategories().Any(c => c.Id == trainCar.CategoryId))
            {
                ModelState.AddModelError("Invalid Category", "Category is invalid.");
            }

            if (!Uri.IsWellFormedUriString(trainCar.Picture, UriKind.Absolute))
            {
                ModelState.AddModelError("Invalid Url", "Url is invalid.");
            }

            if (!ModelState.IsValid)
            {
                trainCar.Interrails = service.AllInterrails();
                trainCar.Categories = service.AllCategories();
                return View(trainCar);
            }

            var trainCarId = service.Create(
                trainCar.Model,
                trainCar.Year.Value,
                trainCar.Series.Value,
                trainCar.CategoryId.Value,
                trainCar.SeatCount.Value,
                parsedLuxuryType,
                trainCar.InterrailId.Value,
                trainCar.Picture,
                trainCar.Description,
                trainCar.Price.Value);

            return Redirect(TrainCarAllRoute);
        }

        public IActionResult Edit(int id)
        {

            var trainCar = service.FormDetails(id);

            if (trainCar == null)
            {
                return Redirect(TrainCarAllRoute);
            }

            trainCar.Interrails = service.AllInterrails();
            trainCar.Categories = service.AllCategories();

            return View(trainCar);
        }

        [HttpPost]
        public IActionResult Edit(int id, TrainCarFormModel trainCar)
        {
            if (!Enum.TryParse(trainCar.LuxuryLevel, out LuxuryLevel parsedLuxuryType))
            {
                ModelState.AddModelError("Invalid luxury level.", "We do not offer this luxury level.");
            }

            if (!service.AllInterrails().Any(i => i.Id == trainCar.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!service.AllCategories().Any(c => c.Id == trainCar.CategoryId))
            {
                ModelState.AddModelError("Invalid Category", "Category is invalid.");
            }

            if (!Uri.IsWellFormedUriString(trainCar.Picture, UriKind.Absolute))
            {
                ModelState.AddModelError("Invalid Url", "Url is invalid.");
            }

            if (!ModelState.IsValid)
            {
                trainCar.Interrails = service.AllInterrails();
                trainCar.Categories = service.AllCategories();
                return View(trainCar);
            }

            var isEdited = service.Edit(
                id,
                trainCar.Model,
                trainCar.Year.Value,
                trainCar.Series.Value,
                trainCar.CategoryId.Value,
                trainCar.SeatCount.Value,
                parsedLuxuryType,
                trainCar.InterrailId.Value,
                trainCar.Picture,
                trainCar.Description,
                trainCar.Price.Value
                );

            if (!isEdited)
            {
                trainCar.Interrails = service.AllInterrails();
                trainCar.Categories = service.AllCategories();
                return View(trainCar);
            }

            return Redirect(TrainCarAllRoute);
        }

        public IActionResult Delete(int id)
        {
            var isRemoved = service.Remove(id);

            if (isRemoved)
            {
                return Redirect("/Home/Trains");
            }

            return Redirect(TrainCarAllRoute);
        }
    }
}
