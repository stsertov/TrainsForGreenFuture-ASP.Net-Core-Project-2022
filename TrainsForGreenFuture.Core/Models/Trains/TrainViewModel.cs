namespace TrainsForGreenFuture.Core.Models.Trains
{
    public class TrainViewModel
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public int Series { get; set; }

        public string LuxuryLevel { get; set; }

        public int TrainCarCount { get; set; }

        public string EngineType { get; set; }

        public int InterrailLength { get; set; }

        public int TopSpeed { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
