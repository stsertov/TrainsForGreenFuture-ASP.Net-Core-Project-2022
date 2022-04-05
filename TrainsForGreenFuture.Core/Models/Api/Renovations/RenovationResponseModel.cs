namespace TrainsForGreenFuture.Core.Models.Api.Renovations
{
    using TrainsForGreenFuture.Core.Models.Api.Locomotives;
    using TrainsForGreenFuture.Core.Models.Api.TrainCars;
    using TrainsForGreenFuture.Infrastructure.Data.Models.Enum;

    public class RenovationResponseModel
    {
        public string RenovationVolume { get; set; }

        public RenovationType RenovationType { get; set; }

        public LocomotiveResponseModel? Locomotive { get; set; }

        public TrainCarResponseModel? TrainCar { get; set; }

        public int? Deadline { get; set; }

        public decimal? Price { get; set; }

        public string? RenovatedPicture { get; set; }

        public string? Comment { get; set; }
    }
}
