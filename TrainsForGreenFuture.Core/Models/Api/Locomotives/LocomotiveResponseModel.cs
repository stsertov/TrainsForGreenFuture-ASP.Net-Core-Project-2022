namespace TrainsForGreenFuture.Core.Models.Api.Locomotives
{
    public class LocomotiveResponseModel
    {
        public string Model { get; init; }

        public int Year { get; init; }

        public int Series { get; init; }

        public string EngineType { get; init; }

        public int TopSpeed { get; init; }

        public string Picture { get; init; }

        public string Description { get; init; }

        public decimal Price { get; init; }

    }
}
