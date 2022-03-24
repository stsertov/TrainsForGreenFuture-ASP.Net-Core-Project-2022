namespace TrainsForGreenFuture.Infrastructure.Data
{
    using System;
    public class DataConstants
    {

        public class Locomotive
        {
            public const int modelMinLength = 3;
            public const int modelMaxLength = 20;
            public const int yearMinValue = 1970;
            public const int yearMaxValue = 2025;
            public const int seriesMinValue = 77000;
            public const int seriesMaxValue = 100000;
            public const int topSpeedMinValue = 140;
            public const int topSpeedMaxValue = 650;
            public const int pictureMaxLength = 600;
            public const int descriptionMinLength = 10;
            public const int descriptionMaxLength = 500;
            public const double priceMinValue = 1000000;
            public const double priceMaxValue = 5000000;
        }
        public class TrainCar
        {
            public const int modelMinLength = 3;
            public const int modelMaxLength = 20;
            public const int yearMinValue = 1970;
            public const int yearMaxValue = 2025;
            public const int seriesMinValue = 33000;
            public const int seriesMaxValue = 60000;
            public const int seatCountMin = 16;
            public const int seatCountMax = 60;
            public const int weightMinValue = 30;
            public const int weightMaxValue = 80;
            public const int pictureMaxLength = 600;
            public const int descriptionMinLength = 10;
            public const int descriptionMaxLength = 500;
            public const double priceMinValue = 1000000;
            public const double priceMaxValue = 8000000;
        }
        public class Train
        {
            public const int modelMinLength = 3;
            public const int modelMaxLength = 20;
            public const int yearMinValue = 1970;
            public const int yearMaxValue = 2025;
            public const int seriesMinValue = 32000;
            public const int seriesMaxValue = 11000;
            public const int trainCarsMin = 3;
            public const int trainCarsMax = 35;
            public const int pictureMaxLength = 600;
            public const int descriptionMinLength = 10;
            public const int descriptionMaxLength = 500;
            public const double priceMinValue = 1000000;
            public const double priceMaxValue = 8000000;
        }

        public class Category
        {
            public const int nameMinLength = 3;
            public const int nameMaxLength = 20;
        }

        public class Interrail
        {
            public const int minLength = 760; 
            public const int maxLength = 1520; 
        }

        public class User
        {
            public const int nameMinLength = 2;
            public const int nameMaxLength = 25;
            public const int passwordMinLength = 3;
            public const int passwordMaxLength = 100;
            public const int companyMinLength = 5;
            public const int companyMaxLength = 35;
        }
    }
}
