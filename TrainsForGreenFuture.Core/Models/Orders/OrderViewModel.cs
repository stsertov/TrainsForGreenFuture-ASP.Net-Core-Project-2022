namespace TrainsForGreenFuture.Core.Models.Orders
{
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Core.Models.Trains;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    public class OrderViewModel
    {
        public string Id { get; set; }

        public OrderType OrderType { get; set; }

        public string Company { get; set; }

        public DateTime OrderDate { get; set; }

        public LocomotiveViewModel? Locomotive { get; set; }

        public TrainCarViewModel? TrainCar { get; set; }

        public TrainViewModel? Train { get; set; }

        public int InterrailLength { get; set; }

        public decimal AdditionalInterrailTax { get; set; }

        public int Count { get; set; }

        public LuxuryLevel? LuxuryLevel { get; set; }

        public decimal AdditionalLuxuryLevelTax { get; set; }

        public bool IsApproved { get; set; }

        public bool IsPaid { get; set; }

    }
}
