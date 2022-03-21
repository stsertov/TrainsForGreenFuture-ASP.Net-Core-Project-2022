namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class TrainCar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        public int Series { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int Interrail { get; set; }

        public int? SeatCount { get; set; }

        public int? MaxWeightCapacity { get; set; }

        public LuxuryLevel? LuxuryLevel { get; set; }

        [Required]
        public string Picture { get; set; }
    }
}
