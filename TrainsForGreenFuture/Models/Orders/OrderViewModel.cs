namespace TrainsForGreenFuture.Models.Orders
{
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;
    using TrainsForGreenFuture.Models.Locomotives;
    public class OrderViewModel
    {
        public string Id { get; set; }

        public string OrderType { get; set; }

        public DateTime OrderDate { get; set; }

        public LocomotiveViewModel Locomotive { get; set; }

        //To Add TrainCarViewModel
        //       TrainViewModel
        public int InterrailLength { get; set; }

        public decimal AdditionalInterrailTax { get; set; }

        public int Count { get; set; }

        public LuxuryLevel? LuxuryLevel { get; set; }

        public decimal AdditionalLuxuryLevelTax { get; set; }

    }
}
