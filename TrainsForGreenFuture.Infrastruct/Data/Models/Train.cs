namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class Train
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        public int Series { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int TrainCarCount { get; set; }

        public LuxuryLevel LuxuryLevel { get; set; }

        [Required]
        public EngineType EngineType { get; set; }

        public int Interrail { get; set; }

        [Required]
        public string Picture { get; set; }

        public decimal Price { get; set; }
    }
}
