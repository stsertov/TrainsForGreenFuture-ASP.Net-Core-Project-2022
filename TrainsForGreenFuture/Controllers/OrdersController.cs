namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Orders;
    using TrainsForGreenFuture.Extensions;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using static Areas.RolesConstants;

    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderService service;
        private ILocomotiveService locomotiveService;
        private ITrainCarService trainCarService;
        private IMapper mapper;

        public OrdersController(
            IOrderService service,
            ILocomotiveService locomotiveService,
            ITrainCarService trainCarService,
            IMapper mapper)
        {
            this.service = service;
            this.locomotiveService = locomotiveService;
            this.trainCarService = trainCarService;
            this.mapper = mapper;
        }

        public IActionResult MyOrders()
            => View(service.All(User.Id()));

        public IActionResult OrderLocomotive(int id)
        {
            var locomotive = locomotiveService.Details(id);

            if (locomotive == null)
            {
                return RedirectToAction(
                    nameof(LocomotivesController.All),
                    nameof(LocomotivesController)
                    .Replace(nameof(Controller), string.Empty));
            }

            return View(new OrderLocomotiveFormModel
            {
                Count = 1,
                Interrails = locomotiveService.AllInterrails(),
                Locomotive = locomotive,
                InterrailLength = locomotive.Interrail
            });
        }

        [HttpPost]
        public IActionResult OrderLocomotive(OrderLocomotiveFormModel order)
        {
            var locomotive = locomotiveService.Details(order.LocomotiveId.Value);

            if (locomotive == null)
            {
                ModelState.AddModelError("Invalid data", "You should type valid parameters.");
            }

            if (!ModelState.IsValid)
            {
                order.Interrails = locomotiveService.AllInterrails();
                return RedirectToAction(
                    nameof(LocomotivesController.All),
                    nameof(LocomotivesController)
                    .Replace(nameof(Controller), string.Empty));
            }

            decimal additionalTax = order.InterrailLength == locomotive.Interrail ? 0m : 500000m;

            var orderId = service.CreateLocomotiveOrder(
                User.Id(),
                locomotive.Id,
                order.InterrailLength,
                additionalTax,
                order.Count);

            return Redirect("/Orders/MyOrders");
        }

        public IActionResult OrderTrainCar(int id)
        {
            var trainCar = trainCarService.Details(id);

            if (trainCar == null)
            {
                return RedirectToAction(
                    nameof(TrainCarsController.All),
                    nameof(TrainCarsController)
                    .Replace(nameof(Controller), string.Empty));
            }

            return View(new OrderTrainCarFormModel
            {
                Count = 1,
                Interrails = trainCarService.AllInterrails(),
                TrainCar = trainCar,
                InterrailLength = trainCar.InterraiLength,
                LuxuryLevel = trainCar.LuxuryLevel.ToString()
            });
        }

        [HttpPost]
        public IActionResult OrderTrainCar(OrderTrainCarFormModel order)
        {
            var trainCar = trainCarService.Details(order.TrainCarId.Value);

            if (trainCar == null)
            {
                ModelState.AddModelError("Invalid data", "You should type valid parameters.");
            }

            if(!Enum.TryParse<LuxuryLevel>(order.LuxuryLevel, out var parsedLuxuryLevel))
            {
                ModelState.AddModelError("Ivalid luxury level.", "We do not offer this luxury level.");
            }

            if (!ModelState.IsValid)
            {
                order.Interrails = trainCarService.AllInterrails();
                return RedirectToAction(
                    nameof(TrainCarsController.All),
                    nameof(TrainCarsController)
                    .Replace(nameof(Controller), string.Empty));
            }

            decimal additionalInterrailTax = order.InterrailLength == trainCar.InterraiLength ? 0m : 350000m;

            decimal additionalLuxuryTax = (order.LuxuryLevel == "Low" && order.LuxuryLevel != trainCar.LuxuryLevel) ? -200000m : 
                order.LuxuryLevel == trainCar.LuxuryLevel ? 0m : 600000m;

            var orderId = service.CreateTrainCarOrder(
                User.Id(),
                trainCar.Id,
                order.InterrailLength,
                additionalInterrailTax,
                parsedLuxuryLevel,
                additionalLuxuryTax,
                order.Count);

            return Redirect("/Orders/MyOrders");
        }

        public IActionResult Pay(string id)
        {
            
            var IsPaidStatus = service.ChangePaidStatus(id);

            if(IsPaidStatus)
            {
                return Redirect("/Orders/MyOrders");
            }

            return Redirect("/Home");
        }
    }
}
