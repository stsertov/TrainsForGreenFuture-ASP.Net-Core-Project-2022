namespace TrainsForGreenFuture.Core.Models.Renovations
{
    using System;
    using TrainsForGreenFuture.Core.Models.Locomotives;
    using TrainsForGreenFuture.Core.Models.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class RenovationDetailsViewModel
    {
        public string Id { get; set; }

        public RenovationVolume RenovationVolume { get; set; }

        public RenovationType RenovationType { get; set; }

        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }

        public LocomotiveViewModel? Locomotive { get; set; }

        public TrainCarViewModel? TrainCar { get; set; }

        public int? Deadline { get; set; }

        public decimal? Price { get; set; }

        public string? RenovatedPicture { get; set; }

        public string? Comment { get; set; }

        public bool IsApproved { get; set; } = false;

        public bool IsPaid { get; set; } = false;

        public bool IsCancelled { get; set; } = false;
    }
}
