namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class Locomotive
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        public int Year { get; set; }

        public int Series { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public EngineType EngineType { get; set; }

        public int Interrail { get; set; }

        public int TopSpeed { get; set; }

        [Required]
        public string Picture { get; set; }
    }
}
