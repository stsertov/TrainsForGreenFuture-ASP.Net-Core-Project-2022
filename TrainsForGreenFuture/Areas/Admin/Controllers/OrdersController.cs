namespace TrainsForGreenFuture.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TrainsForGreenFuture.Core.Contracts;

    public class OrdersController : AdminController
    {
        private IOrderService service;

        public OrdersController(IOrderService service)
            => this.service = service;
        
        public IActionResult All()
        {
            var orders = service.All();

            return View(orders);
        }

        public IActionResult Approve(string id)
        {
            var isApproved = service.ChangeStatus(id);

            if (isApproved)
            {
                return Redirect("/Admin/Orders/All");
            }

            return Redirect("/Admin/Orders/All");
        }
    }
}
