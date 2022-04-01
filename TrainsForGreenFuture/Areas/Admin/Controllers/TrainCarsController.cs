﻿namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class TrainCarsController : AdminController
    {
        ITrainCarService service;

        public TrainCarsController(ITrainCarService service)
        {
            this.service = service;
        }

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
                ModelState.AddModelError(trainCar.LuxuryLevel, "We do not offer this type of train car.");
            }

            if (!service.AllInterrails().Any(i => i.Id == trainCar.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!service.AllCategories().Any(c => c.Id == trainCar.CategoryId))
            {
                ModelState.AddModelError("Invalid Category", "Category is invalid.");
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

            return Redirect("/TrainCars/All");
        }

        public IActionResult Edit(int id)
        {

            var trainCar = service.FormDetails(id);

            if (trainCar == null)
            {
                return Redirect("/TrainCars/All");
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
                ModelState.AddModelError(trainCar.LuxuryLevel, "We do not offer this type of train car.");
            }

            if (!service.AllInterrails().Any(i => i.Id == trainCar.InterrailId))
            {
                ModelState.AddModelError("Invalid Interrail", "Interrail is invalid.");
            }

            if (!service.AllCategories().Any(c => c.Id == trainCar.CategoryId))
            {
                ModelState.AddModelError("Invalid Category", "Category is invalid.");
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

            return Redirect("/TrainCars/All");
        }

        public IActionResult Delete(int id)
        {
            var isRemoved = service.Remove(id);

            if (isRemoved)
            {
                return Redirect("/Home/Trains");
            }

            return Redirect("/TrainCars/All");
        }
    }
}
