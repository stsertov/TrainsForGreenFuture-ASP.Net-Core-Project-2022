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
        private const string LocomotiveOrder = "Locomotive";
        private const string TrainCarOrder = "TrainCar";
        private const string TrainOrder = "Train";
        private TrainsDbContext context;
        private IMapper mapper;

        public OrderService(TrainsDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<OrderViewModel> All()
        {
            var orders = GetAllOrders().ToList();

            return mapper.Map<List<OrderViewModel>>(orders);
        }
        public IEnumerable<OrderViewModel> All(string userId)
        {
            var orders = GetAllOrders()
                .Where(o => o.UserId == userId)
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
                OrderType = Enum.Parse<OrderType>(LocomotiveOrder),
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
        public string CreateTrainCarOrder(
            string userId,
            int trainCarId,
            int interrailLength,
            decimal additionalInterrailtax,
            LuxuryLevel luxuryLevel,
            decimal additionalLuxuryLeveltax,
            int count)
        {
            var order = new Order
            {
                OrderType = Enum.Parse<OrderType>(TrainCarOrder),
                OrderDate = DateTime.UtcNow,
                UserId = userId,
                TrainCarId = trainCarId,
                InterrailLength = interrailLength,
                AdditionalInterrailTax = additionalInterrailtax,
                LuxuryLevel = luxuryLevel,
                AdditionalLuxuryLevelTax = additionalLuxuryLeveltax,
                Count = count
            };

            context.Orders.Add(order);
            context.SaveChanges();

            return order.Id;
        }
        public string CreateTrainOrder(
            string userId,
            int trainId,
            int interrailLength,
            decimal additionalInterrailtax,
            LuxuryLevel luxuryLevel,
            decimal additionalLuxuryLeveltax,
            int count)
        {
            var order = new Order
            {
                OrderType = Enum.Parse<OrderType>(TrainOrder),
                OrderDate = DateTime.UtcNow,
                UserId = userId,
                TrainId = trainId,
                InterrailLength = interrailLength,
                AdditionalInterrailTax = additionalInterrailtax,
                LuxuryLevel = luxuryLevel,
                AdditionalLuxuryLevelTax = additionalLuxuryLeveltax,
                Count = count
            };

            context.Orders.Add(order);
            context.SaveChanges();

            return order.Id;
        }

        public bool ChangeStatus(string orderId)
        {
            var order = GetOrder(orderId);

            if (order == null)
            {
                return false;
            }

            order.IsApproved = true;

            context.SaveChanges();

            return true;
        }

        public bool ChangePaidStatus(string orderId)
        {
            var order = GetOrder(orderId);

            if (order == null ||
                !order.IsApproved)
            {
                return false;
            }

            order.IsPaid = true;

            context.SaveChanges();

            return true;
        }

        private Order GetOrder(string orderId)
            => context.Orders
                .FirstOrDefault(o => o.Id == orderId);

        private IQueryable<Order> GetAllOrders()
            => context.Orders
                .Include(o => o.User)
                .Include(o => o.Locomotive)
                .Include(o => o.Train)
                .Include(o => o.TrainCar)
                .ThenInclude(tc => tc.Category)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();
    }
}

