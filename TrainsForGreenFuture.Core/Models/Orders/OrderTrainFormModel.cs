namespace TrainsForGreenFuture.Core.Models.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using TrainsForGreenFuture.Core.Models.Trains;
    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.Order;
    public class OrderTrainFormModel
    {
        public TrainViewModel? Train { get; set; }

        public int? TrainId { get; set; }

        public int InterrailLength { get; set; }

        public string LuxuryLevel { get; set; }

        [Range(countMinValue, countMaxValue)]
        public int Count { get; set; }

        public IEnumerable<InterrailServiceModel>? Interrails { get; set; }
    }
}
