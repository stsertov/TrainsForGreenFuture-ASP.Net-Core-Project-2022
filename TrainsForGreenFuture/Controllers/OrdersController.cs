namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Orders;
    using TrainsForGreenFuture.Extensions;
    using static Areas.RolesConstants;

    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderService service;
        private ILocomotiveService locomotiveService;
        private IMapper mapper;

        public OrdersController(
            IOrderService service,
            ILocomotiveService locomotiveService,
            IMapper mapper)
        {
            this.service = service;
            this.locomotiveService = locomotiveService;
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
