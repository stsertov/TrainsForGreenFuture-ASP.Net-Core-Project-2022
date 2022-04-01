namespace TrainsForGreenFuture.Core.Models.Orders
{
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.Order;
    public class OrderTrainCarFormModel
    {
        public TrainCarViewModel? TrainCar { get; set; }

        public int? TrainCarId { get; set; }

        public int InterrailLength { get; set; }

        public string LuxuryLevel { get; set; }

        [Range(countMinValue, countMaxValue)]
        public int Count { get; set; }

        public IEnumerable<InterrailServiceModel>? Interrails { get; set; }

    }
}
