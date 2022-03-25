namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    using static DataConstants.Locomotive;
    public class Locomotive
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(modelMaxLength, MinimumLength = modelMinLength)]
        public string Model { get; set; }

        [Range(yearMinValue, yearMaxValue)]
        public int Year { get; set; }

        [Range(seriesMinValue, seriesMaxValue)]
        public int Series { get; set; }

        [Required]
        public EngineType EngineType { get; set; }

        [ForeignKey(nameof(Interrail))]
        public int InterrailId { get; set; }
        public Interrail Interrail { get; set; }

        [Range(topSpeedMinValue, topSpeedMaxValue)]
        public int TopSpeed { get; set; }

        [Required]
        [StringLength(pictureMaxLength)]
        public string Picture { get; set; }

        [StringLength(descriptionMaxLength, MinimumLength = descriptionMinLength)]
        public string Description { get; set; }

        [Range(priceMinValue, priceMaxValue)]
        public decimal Price { get; set; }

        public bool IsForRenovation { get; set; } = false;
    }
}
