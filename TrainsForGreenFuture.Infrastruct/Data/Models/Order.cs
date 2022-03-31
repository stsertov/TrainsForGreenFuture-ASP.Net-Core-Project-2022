namespace TrainsForGreenFuture.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TrainsForGreenFuture.Infrastructure.Data.Identity;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    using static Data.DataConstants.Order;
    public class Order
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public OrderType OrderType { get; set; }
        
        public DateTime OrderDate { get; set; }

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

        [ForeignKey(nameof(Train))]
        public int? TrainId { get; set; }

        public Train? Train { get; set; }

        public int InterrailLength { get; set; }

        public decimal AdditionalInterrailTax { get; set; }

        [Range(countMinValue, countMaxValue)]
        public int Count { get; set; }

        public LuxuryLevel? LuxuryLevel { get; set; }

        public decimal AdditionalLuxuryLevelTax { get; set; }

        public bool IsApproved { get; set; } = false;

        public bool IsPaid { get; set; } = false;
    }
}
