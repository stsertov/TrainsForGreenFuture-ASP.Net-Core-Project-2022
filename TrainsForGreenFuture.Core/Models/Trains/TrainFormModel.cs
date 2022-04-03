namespace TrainsForGreenFuture.Core.Models.Trains
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Core.Models.Interrails;
    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.Train;
    public class TrainFormModel
    {
        [Required]
        [StringLength(modelMaxLength, MinimumLength = modelMinLength)]
        public string Model { get; set; }

        [Required]
        [Range(yearMinValue, yearMaxValue)]
        public int? Year { get; set; }

        [Required]
        [Range(seriesMinValue, seriesMaxValue)]
        public int? Series { get; set; }

        [Required]
        public string LuxuryLevel { get; set; }

        [Required]
        [Range(trainCarsMin, trainCarsMax)]
        public int? TrainCarCount { get; set; }

        [Required]
        public string EngineType { get; set; }

        [Required]
        public int? InterrailId { get; set; }

        [Required]
        [Range(topSpeedMinValue, topSpeedMaxValue)]
        public int? TopSpeed { get; set; }

        [Required]
        [StringLength(pictureMaxLength)]
        public string Picture { get; set; }

        [StringLength(descriptionMaxLength, MinimumLength = descriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Range(priceMinValue, priceMaxValue)]
        public decimal? Price { get; set; }

        public IEnumerable<InterrailServiceModel>? Interrails { get; set; }
    }
}
