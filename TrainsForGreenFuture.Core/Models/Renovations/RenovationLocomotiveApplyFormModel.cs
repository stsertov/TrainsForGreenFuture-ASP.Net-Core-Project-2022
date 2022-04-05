namespace TrainsForGreenFuture.Core.Models.Renovations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Core.Models.Interrails;

    using static TrainsForGreenFuture.Infrastructure.Data.DataConstants.Locomotive;
    public class RenovationLocomotiveApplyFormModel
    {
        public string RenovationVolume { get; set; }

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
        [StringLength(pictureMaxLength)]
        public string Picture { get; init; }


        [StringLength(descriptionMaxLength, MinimumLength = descriptionMinLength)]
        public string Description { get; init; }

        public IEnumerable<InterrailServiceModel>? Interrails { get; set; }
    }
}
