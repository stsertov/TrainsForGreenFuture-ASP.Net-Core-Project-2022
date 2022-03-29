namespace TrainsForGreenFuture.Models.Orders
{
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using TrainsForGreenFuture.Models.Locomotives;

    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.Order;
    public class OrderLocomotiveFormModel
    {

        public LocomotiveViewModel? Locomotive { get; set; }

        public int? LocomotiveId { get; set; }

        public int InterrailLength { get; set; }

        [Range(countMinValue, countMaxValue)]
        public int Count { get; set; }

        public IEnumerable<Interrail>? Interrails { get; set; }
    }
}
