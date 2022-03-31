namespace TrainsForGreenFuture.Core.Models.TrainCars
{
    public class TrainCarViewModel
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }
        public int Series { get; set; }

        public string CategoryName { get; set; }

        public int SeatCount { get; set; }

        public string LuxuryLevel { get; set; }


        public int InterraiLength { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

    }
}
