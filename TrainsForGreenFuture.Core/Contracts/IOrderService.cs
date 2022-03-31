namespace TrainsForGreenFuture.Core.Contracts
{
    using TrainsForGreenFuture.Core.Models.Orders;

    public interface IOrderService
    {
        public IEnumerable<OrderViewModel> All();
        public IEnumerable<OrderViewModel> All(string userId);

        public string CreateLocomotiveOrder(
            string userId,
            int locomotiveId,
            int interrailLength,
            decimal additionalInterrailtax,
            int count);

        public bool ChangeStatus(string orderId);

        public bool ChangePaidStatus(string orderId);
    }
}
