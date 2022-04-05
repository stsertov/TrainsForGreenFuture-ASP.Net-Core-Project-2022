namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    using static DataConstants.Renovation;
    public class Renovation
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public RenovationVolume RenovationVolume { get; set; }

        public RenovationType RenovationType { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Locomotive))]
        public int? LocomotiveId { get; set; }

        public Locomotive? Locomotive { get; set; }

        [ForeignKey(nameof(TrainCar))]
        public int? TrainCarId { get; set; }

        public TrainCar? TrainCar { get; set; }

        [Range(minDeadline, maxDeadline)]
        public int? Deadline { get; set; }

        [Range(minPriceValue, maxPriceValue)]
        public decimal? Price { get; set; }

        public string RenovatedPicture { get; set; }

        public string Comment { get; set; }

        public bool IsApproved { get; set; } = false;

        public bool IsPaid { get; set; } = false;

        public bool IsCancelled { get; set; } = false;
    }
}
