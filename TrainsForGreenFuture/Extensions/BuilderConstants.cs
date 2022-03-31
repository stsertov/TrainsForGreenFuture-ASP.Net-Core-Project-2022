namespace TrainsForGreenFuture.Extensions
{
    public class BuilderConstants
    {
        // series 77 000 - 100 000
        public class Corvus
        {
            public const string Model = "Corvus";
            public const int Year = 2021;
            public const int Series = 99002;
            public const string EngineType = "Electric";
            public const int Interrail = 1435;
            public const int TopSpeed = 250;
            public const string Image = "https://bit.ly/3qMWlZt";
            public const string Description = "Corvus is good for long distance jorneys and big cargo train compositions. Pretty strong and fast. Can operate with more than 100 train cars.";
            public const decimal Price = 3500000m;
        }
        public class Orion
        {
            public const string Model = "Orion";
            public const int Year = 2022;
            public const int Series = 99804;
            public const string EngineType = "Electric";
            public const int Interrail = 1435;
            public const int TopSpeed = 300;
            public const string Image = "https://bit.ly/3J2j1eU";
            public const string Description = "Orion is our most ambitiuos locomotive so far. It has everything you would want from a locomotive.";
            public const decimal Price = 4500000m;
        }
        public class Leo
        {
            public const string Model = "Leo";
            public const int Year = 2019;
            public const int Series = 85350;
            public const string EngineType = "Hybrid";
            public const int Interrail = 1435;
            public const int TopSpeed = 250;
            public const string Image = "https://bit.ly/3DryCDq";
            public const string Description = "Leo is our hybrid locomotive. Both strong and eco it is good solution for far away travels where the electric net is not build yet.";
            public const decimal Price = 4000000m;
        }

        //33 000 - 60 000
        public class Snorlax
        {
            public const string Model = "Snorlax";
            public const int Year = 2021;
            public const int Series = 59921;
            public const string Category = "Sleeper";
            public const int SeatCount = 30;
            public const string LuxuryLevel = "Luxury";
            public const int InterrailLength = 1435;
            public const string Picture = "https://bit.ly/3iQE6hp";
            public const string Description = "Our most luxury sleepr model. The beds are so good it's like you're laying on clouds. Also this train car has small cafeteria place in it.";
            public const decimal Price = 7200000m;
        }

        public class SoftUniTrainer
        {
            public const string Model = "SU Trainer Car";
            public const int Year = 2022;
            public const int Series = 33508;
            public const string Category = "Education";
            public const int SeatCount = 55;
            public const string LuxuryLevel = "Luxury";
            public const int InterrailLength = 1435;
            public const string Picture = "https://bit.ly/3NAg5cJ";
            public const string Description = "Even SoftUni are using our services for their mobile educational program.";
            public const decimal Price = 5800000m;
        }
    }
}
