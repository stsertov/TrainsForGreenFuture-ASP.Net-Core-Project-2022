namespace TrainsForGreenFuture.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TrainsForGreenFuture.Extensions;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Models.Locomotives;
    using TrainsForGreenFuture.Models.Orders;
    using static Areas.RolesConstants;
    [Authorize]
    public class OrdersController : Controller
    {
        private TrainsDbContext context;
        private IMapper mapper;

        public OrdersController(TrainsDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult MyOrders()
            => View();

        public IActionResult OrderLocomotive(int id)
        {
            var dbLocomotive = context.Locomotives
                .Include(l => l.Interrail)
                .FirstOrDefault(l => !l.IsForRenovation && l.Id == id);

            if (dbLocomotive == null)
            {
                return RedirectToAction(
                    nameof(LocomotivesController.All),
                    nameof(LocomotivesController)
                    .Replace(nameof(Controller), string.Empty));
            }


            var order = new OrderLocomotiveFormModel
            {
                Count = 1,
                Interrails = context.Interrails.ToList(),
                Locomotive = mapper.Map<LocomotiveViewModel>(dbLocomotive),
                InterrailLength = dbLocomotive.Interrail.Length
            };

            return View(order);
        }

        [HttpPost]
        [Authorize]
        public IActionResult OrderLocomotive(OrderLocomotiveFormModel order)
        {
            var dbLocomotive = context.Locomotives
                .Include(l => l.Interrail)
                .FirstOrDefault(l => !l.IsForRenovation && l.Id == order.LocomotiveId);

            if (dbLocomotive == null)
            {
                ModelState.AddModelError("Invalid data", "You should type valid paramters.");
            }

            if (!ModelState.IsValid)
            {
                order.Interrails = context.Interrails.ToList();
                return RedirectToAction(
                    nameof(LocomotivesController.All),
                    nameof(LocomotivesController)
                    .Replace(nameof(Controller), string.Empty));
            }

            decimal additionalTax = order.InterrailLength == dbLocomotive.Interrail.Length ? 0m : 500000m;

            context.Orders.Add(new Order
            {
                OrderType = 0,
                OrderDate = DateTime.UtcNow,
                UserId = User.Id(),
                Locomotive = dbLocomotive,
                InterrailLength = order.InterrailLength,
                AdditionalInterrailTax = additionalTax,
                Count = order.Count
            });
            context.SaveChanges();

            return Redirect("/Home/Trains");
        }

        [Authorize(Roles = $"{AdministratorRole}, {EngineerRole}")]
        public IActionResult Approve()
        {
            return Redirect("/Home");
        }
    }
}
