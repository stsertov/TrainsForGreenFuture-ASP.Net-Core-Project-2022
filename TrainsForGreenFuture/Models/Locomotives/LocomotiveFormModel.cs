using TrainsForGreenFuture.Infrastructure.Data;

namespace TrainsForGreenFuture.Models.Locomotives
{
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Infrastructure.Data.Models;
    using static DataConstants.Locomotive;
    public class LocomotiveFormModel
    {
        [Required]
        [StringLength(modelMaxLength, MinimumLength = modelMinLength)]
        public string Model { get; init; }

        [Required]
        [Range(yearMinValue, yearMaxValue)]
        public int? Year { get; init; }


        [Required]
        [Range(seriesMinValue, seriesMaxValue)]
        [RegularExpression(@"\b[0-9]{5,6}\b")]
        public int? Series { get; init; }

        [Required]
        public string EngineType { get; init; }

        [Required]
        public int? InterrailId { get; init; }

        [Required]
        [Range(topSpeedMinValue, topSpeedMaxValue)]
        public int? TopSpeed { get; init; }

        [Required]
        [StringLength(pictureMaxLength)]
        public string Picture { get; init; }

        [StringLength(descriptionMaxLength, MinimumLength = descriptionMinLength)]
        public string Description { get; init; }

        [Required]
        [Range(priceMinValue, priceMaxValue)]
        public decimal? Price { get; init; }

        public bool IsForRenovation { get; init; }

        public IEnumerable<Interrail>? Interrails { get; set; }
    }
}
