namespace TrainsForGreenFuture.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using TrainsForGreenFuture.Core.Contracts;
    using TrainsForGreenFuture.Core.Models.Orders;
    using TrainsForGreenFuture.Infrastructure.Data;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class OrderService : IOrderService
    {
        private const string orderType = "Locomotive";
        private TrainsDbContext context;
        private IMapper mapper;

        public OrderService(TrainsDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IEnumerable<OrderViewModel> All(string userId)
        {
            var orders = context.Orders
                .Include(o => o.User)
                .Include(o => o.Locomotive)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return mapper.Map<List<OrderViewModel>>(orders);
        }

        public string CreateLocomotiveOrder(
            string userId,
            int locomotiveId,
            int interrailLength,
            decimal additionalInterrailtax,
            int count)
        {
            var order = new Order
            {
                OrderType = Enum.Parse<OrderType>(orderType),
                OrderDate = DateTime.UtcNow,
                UserId = userId,
                LocomotiveId = locomotiveId,
                InterrailLength = interrailLength,
                AdditionalInterrailTax = additionalInterrailtax,
                Count = count
            };

            context.Orders.Add(order);
            context.SaveChanges();

            return order.Id;
        }


    }
}

