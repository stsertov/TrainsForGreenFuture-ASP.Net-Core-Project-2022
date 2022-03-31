namespace TrainsForGreenFuture.Core.Models.Locomotives
{
    public class LocomotiveViewModel
    {
        public int Id { get; init; }
        public string Model { get; init; }

        public int Year { get; init; }

        public int Series { get; init; }

        public string EngineType { get; init; }

        public int Interrail { get; init; }

        public int TopSpeed { get; init; }

        public string Picture { get; init; }

        public string Description { get; init; }

        public decimal Price { get; init; }
    }
}
