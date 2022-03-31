namespace TrainsForGreenFuture.Core.Models.TrainCars
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Core.Models.Categories;
    using TrainsForGreenFuture.Core.Models.Interrails;

    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.TrainCar;
    public class TrainCarFormModel
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
        public int? CategoryId { get; set; }

        [Required]
        [Range(seatCountMin, seatCountMax)]
        public int? SeatCount { get; set; }

        [Required]
        public string LuxuryLevel { get; set; }

        [Required]
        public int? InterrailId { get; set; }

        [Required]
        [StringLength(pictureMaxLength)]
        public string Picture { get; set; }

        [StringLength(descriptionMaxLength, MinimumLength = descriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Range(priceMinValue, priceMaxValue)]
        public decimal? Price { get; set; }

        public IEnumerable<CategoryServiceModel>? Categories { get; set; }

        public IEnumerable<InterrailServiceModel>? Interrails { get; set; }

    }
}
