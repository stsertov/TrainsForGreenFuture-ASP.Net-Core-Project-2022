namespace TrainsForGreenFuture.Extensions
{
    public class BuilderConstants
    {
        // series 77000 - 100 000
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
    }
}
